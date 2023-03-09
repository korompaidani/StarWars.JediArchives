namespace StarWars.JediArchives.Infrastructure.QueryParser
{
    //TODO: Separate classes and thinking on visibility
    public enum Comparer
    {
        Equal,
        Less,
        Greater
    }

    public enum OrderBy
    {
        Ascend,
        Descend
    }

    public class QueryProcessorBuilder : IBuilder<QueryProcessor>
    {
        private RuleBuilder _ruleBuilder;
        private Type _targetType;
        private QueryProcessor _queryParser;
        private HashSet<string> _propertyCollection;
        private List<RuleBuilder> _ruleBuilders;

        public RuleBuilder WithNewFilteredRule(string filter)
        {
            _ruleBuilder = new RuleBuilder(this, filter, _propertyCollection);
            _ruleBuilders.Add(_ruleBuilder);
            return _ruleBuilder;
        }

        public QueryProcessorBuilder(Type targetType)
        {
            _ruleBuilders = new List<RuleBuilder>();
            _targetType = targetType;
            _propertyCollection = _targetType.GetProperties().Select(a => a.Name).ToHashSet();
        }

        public QueryProcessor Build()
        {
            _queryParser = new QueryProcessor(_propertyCollection);

            foreach (var ruleBuilder in _ruleBuilders)
            {
                var rule = ruleBuilder.Build();
                _queryParser.AddProcess(rule.Key, rule.Value);
            }

            return _queryParser;
        }
    }

    public class RuleBuilder : IBuilder<KeyValuePair<string, Func<string, QueryOperation>>>
    {
        private QueryProcessorBuilder _queryParserBuilder;

        private string _filter;
        private char _valueFromCharacter;
        private int _valueUntilEndIndex;
        private int _propertyFromIndex;
        private char _propertyEndChar;
        private string _orderByPropertyName;
        private OrderBy _orderByDirection;

        private HashSet<string> _propertyCollection;
        private Func<int, int, bool> _comparer;
        private Func<dynamic, dynamic> _orderByQuery;
        private KeyValuePair<string, Func<string, QueryOperation>> _rule;

        public RuleBuilder(QueryProcessorBuilder queryParserBuilder, string filter, HashSet<string> propertyCollection)
        {            
            _queryParserBuilder = queryParserBuilder;
            _filter = filter;
            _propertyCollection = propertyCollection;
        }

        public RuleBuilder WithValueFromCharacterUntilEndIndex(char fromCharacter, int endIndex)
        {
            _valueFromCharacter = fromCharacter;
            _valueUntilEndIndex = endIndex;
            return this;
        }

        public RuleBuilder WithPropertyFromIndexUntilEndCharacter(int fromIndex, char endCharacter)
        {
            _propertyFromIndex = fromIndex;
            _propertyEndChar = endCharacter;
            return this;
        }

        public QueryProcessorBuilder WithExpectedComparer(Comparer expectedComparer)
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

        public QueryProcessorBuilder WithExpectedOrderBy(OrderBy expectedDirection, string propertyName)
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

        public KeyValuePair<string, Func<string, QueryOperation>> Build()
        {
            var operation = CreateComparerQueryOperation(
                _valueFromCharacter,
                _propertyEndChar,
                _propertyFromIndex,
                _valueUntilEndIndex,
                _comparer,
                _propertyCollection);

            _rule = new KeyValuePair<string, Func<string, QueryOperation>>(_filter, operation);
            return _rule;
        }

        #region HelperMethods
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
                throw new Exception();
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
                throw new Exception();
            }

            var result = input.Substring(startIndex, input.IndexOf(untilCharacter) - startIndex);
            return result;
        }

        private int GetIntegerFromValue(string value)
        {
            int integer;
            if (!int.TryParse(value, out integer))
            {
                throw new Exception();
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
                throw new Exception();
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
}