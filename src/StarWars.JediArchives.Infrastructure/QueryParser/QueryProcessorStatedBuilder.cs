namespace StarWars.JediArchives.Infrastructure.QueryParser
{
    public class QueryProcessorStatedBuilder<T> : IQueryProcessorBuilder, IBuilder<QueryProcessor<T>> where T : class
    {
        private RuleBuilder _ruleBuilder;
        private Type _targetType;
        private QueryProcessor<T> _queryProcessor;
        private HashSet<string> _propertyCollection;
        private List<RuleBuilder> _ruleBuilders;

        public IRuleBuilder WithNewFilteredRule(string filter)
        {
            _ruleBuilder = new RuleBuilder(this, filter, _propertyCollection);
            _ruleBuilders.Add(_ruleBuilder);
            return _ruleBuilder;
        }

        /// <summary>
        /// QueryProcessorBuilder for building Processor in fluent manner
        /// </summary>
        /// <param name="targetType">Only integer properties are supported in this version</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="QueryValidationException"></exception>
        public QueryProcessorStatedBuilder(Type targetType)
        {
            if(targetType is null)
            {
                throw new ArgumentNullException(nameof(targetType));
            }

            _ruleBuilders = new List<RuleBuilder>();
            _targetType = targetType;
            _propertyCollection = _targetType.GetProperties().Where(p => p.PropertyType == typeof(int)).Select(a => a.Name).ToHashSet();

            if(_propertyCollection is null || _propertyCollection.Count == 0)
            {
                throw new QueryValidationException(new[] { $"There was no appropriate property in the given type: {targetType.GetType()}. Only {typeof(int)} types are supported in this version."});
            }
        }

        public QueryProcessor<T> Build()
        {
            if (_ruleBuilders.Count == 0)
            {
                throw new QueryValidationException(new[]{"There is no Rule Defined."});
            }

            _queryProcessor = new QueryProcessor<T>();

            foreach (var ruleBuilder in _ruleBuilders)
            {
                var rule = ruleBuilder.Build();
                _queryProcessor.AddProcess(rule.Item1, rule.Item2);
            }

            return _queryProcessor;
        }

        #region Nested RuleBuilder
        public class RuleBuilder : IBuilder<Tuple<string, Func<string, QueryOperation>>>, IRuleBuilder
        {
            private QueryProcessorStatedBuilder<T> _queryParserBuilder;

            private string? _filter;
            private char? _valueFromCharacter;
            private int? _valueUntilEndIndex;
            private int? _propertyFromIndex;
            private char? _propertyEndChar;
            private string _orderByPropertyName;
            private OrderBy _orderByDirection;

            private HashSet<string> _propertyCollection;
            private Func<int, int, bool> _comparer;
            private Func<dynamic, dynamic> _orderByQuery;
            private Tuple<string, Func<string, QueryOperation>> _rule;

            public RuleBuilder(QueryProcessorStatedBuilder<T> queryParserBuilder, string filter, HashSet<string> propertyCollection)
            {
                _queryParserBuilder = queryParserBuilder;
                _filter = filter;
                _propertyCollection = propertyCollection;
            }

            public IRuleBuilder WithValueFromCharacterUntilEndIndex(char fromCharacter, int endIndex)
            {
                _valueFromCharacter = fromCharacter;
                _valueUntilEndIndex = endIndex;
                return this;
            }

            public IRuleBuilder WithPropertyFromIndexUntilEndCharacter(int fromIndex, char endCharacter)
            {
                _propertyFromIndex = fromIndex;
                _propertyEndChar = endCharacter;
                return this;
            }

            public IQueryProcessorBuilder WithExpectedComparer(Comparer expectedComparer)
            {
                switch (expectedComparer)
                {
                    case Comparer.Equal:
                        _comparer = Equal<int>();
                        break;
                    case Comparer.Less:
                        _comparer = Less<int>();
                        break;
                    case Comparer.Greater:
                        _comparer = Greater<int>();
                        break;
                }

                return _queryParserBuilder;
            }

            public IQueryProcessorBuilder WithExpectedOrderBy(OrderBy expectedDirection, string propertyName)
            {
                switch (expectedDirection)
                {
                    case OrderBy.Ascend:
                        _orderByPropertyName = propertyName;
                        //TODO: _orderByQuery
                        break;
                    case OrderBy.Descend:
                        _orderByPropertyName = propertyName;
                        //TODO: _orderByQuery
                        break;
                }

                return _queryParserBuilder;
            }

            public Tuple<string, Func<string, QueryOperation>> Build()
            {
                ValidateProperties(_valueFromCharacter,_propertyEndChar, _propertyFromIndex, _valueUntilEndIndex);
                
                var operation = CreateComparerQueryOperation(
                    _valueFromCharacter.Value,
                    _propertyEndChar.Value,
                    _propertyFromIndex.Value,
                    _valueUntilEndIndex.Value,
                    _comparer,
                    new HashSet<string>(_propertyCollection, StringComparer.OrdinalIgnoreCase)
                    );

                _rule = new Tuple<string, Func<string, QueryOperation>>(_filter, operation);
                return _rule;
            }

            #region HelperMethods
            private void ValidateProperties(char? valueFromCharacter, char? propertyEndChar, int? propertyFromIndex, int? valueUntilEndIndex)
            {
                var possibleExceptionMessages = new List<string>();

                if (valueFromCharacter is null || valueUntilEndIndex is null)
                {
                    possibleExceptionMessages.Add($"{valueFromCharacter} and/or {valueUntilEndIndex} should be set.");
                }
                if (propertyEndChar is null || propertyFromIndex is null)
                {
                    possibleExceptionMessages.Add($"{propertyEndChar} and/or {propertyFromIndex} should be set.");
                }

                if (possibleExceptionMessages.Count != 0)
                {
                    throw new QueryValidationException(possibleExceptionMessages);
                }
            }

            private Func<string, QueryOperation> CreateComparerQueryOperation(char valueFromCharacter, char propertyEndChar, int propertyFromIndex, int valueUntilEndIndex, Func<int, int, bool> comparer, HashSet<string> propertyCollection)
            {
                return delegate (string userDefinedSingleQuery)
                {
                    var val = CreateStringFromCharacterUntilIndexFromEnd(userDefinedSingleQuery, valueFromCharacter, valueUntilEndIndex);
                    var propName = CreateStringFromIndexUntilEndCharacter(userDefinedSingleQuery, propertyEndChar, propertyFromIndex);
                    var filteredPropName = FilterProperties(propName);
                    var num = GetIntegerFromValue(val);
                    var foundProp = GetOriginalPropertyName(filteredPropName, val, num, propertyCollection);
                    var task = SetFilterLinqQuery(num, foundProp, comparer);
                    return new QueryOperation { Value = val, PropertyName = propName, CompareTask = task };
                };
            }

            private string CreateStringFromCharacterUntilIndexFromEnd(string input, char fromCharacter, int characterFromEnd = 0)
            {
                const int offSetValue = 1;
                var indexOfFromCharacter = input.IndexOf(fromCharacter);

                if (indexOfFromCharacter == -1)
                {
                    throw new QueryValidationException(new[] { $"The character '{fromCharacter}' was not found in {nameof(CreateStringFromCharacterUntilIndexFromEnd)}" });
                }

                int length = input.Length - indexOfFromCharacter - offSetValue - characterFromEnd;
                if (!(indexOfFromCharacter + length < input.Length && length > 0))
                {
                    length = input.Length - indexOfFromCharacter - offSetValue;
                }

                var result = input.Substring((indexOfFromCharacter + offSetValue), length);

                return result;
            }

            private string CreateStringFromIndexUntilEndCharacter(string input, char untilCharacter, int startIndex = 0)
            {
                int indexOfUntilCharacter = input.IndexOf(untilCharacter);
                if (indexOfUntilCharacter < 1 || !(startIndex < indexOfUntilCharacter && startIndex > -1))
                {
                    throw new QueryValidationException(new[] { $"The character '{untilCharacter}' was not found or is in first position or invalid {nameof(startIndex)} was set: {startIndex} in: {nameof(CreateStringFromIndexUntilEndCharacter)}" });
                }

                var result = input.Substring(startIndex, input.IndexOf(untilCharacter) - startIndex);
                return result;
            }

            private int GetIntegerFromValue(string value)
            {
                int integer;
                if (!int.TryParse(value, out integer))
                {
                    throw new QueryValidationException(new[] { $"Only {typeof(int)} value properties are supported in this version." });
                }
                return integer;
            }

            private Func<T, T, bool> GreaterOrEqual<T>() where T : IComparable<T>
            {
                return delegate (T lhs, T rhs) { return lhs.CompareTo(rhs) >= 0; };
            }

            private Func<T, T, bool> LessOrEqual<T>() where T : IComparable<T>
            {
                return delegate (T lhs, T rhs) { return lhs.CompareTo(rhs) <= 0; };
            }

            private Func<T, T, bool> Greater<T>() where T : IComparable<T>
            {
                return delegate (T lhs, T rhs) { return lhs.CompareTo(rhs) > 0; };
            }

            private Func<T, T, bool> Less<T>() where T : IComparable<T>
            {
                return delegate (T lhs, T rhs) { return lhs.CompareTo(rhs) < 0; };
            }

            private Func<T, T, bool> Equal<T>() where T : IComparable<T>
            {
                return delegate (T lhs, T rhs) { return lhs.CompareTo(rhs) == 0; };
            }

            private string FilterProperties(string propName)
            {
                var pattern = @"(\s+|@|&|'|\(|\)|<|>|#|_)";
                var filteredPropName = Regex.Replace(propName, pattern, string.Empty).ToLower();
                return filteredPropName;
            }

            private string GetOriginalPropertyName(string filteredPropName, string value, int numberValue, HashSet<string> propertyCollection)
            {
                string foundProp = null;
                if (!propertyCollection.TryGetValue(filteredPropName, out foundProp) || !int.TryParse(value, out numberValue))
                {
                    throw new QueryValidationException(new[] { $"Only {typeof(int)} value properties are supported in this version." });
                }

                return foundProp;
            }

            private static Func<dynamic, bool> SetFilterLinqQuery(int num, string foundProp, Func<int, int, bool> operation)
            {
                return entity =>
                {
                    var entityProperty = entity.GetType().GetProperty(foundProp);
                    var entityPropertyValue = entityProperty.GetValue(entity);

                    return operation(entityPropertyValue, num);
                };
            }
            #endregion
        }
        #endregion
    }
}