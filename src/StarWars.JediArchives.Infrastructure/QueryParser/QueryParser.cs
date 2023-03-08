using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace StarWars.JediArchives.Infrastructure.QueryParser
{
    public class QueryParser
    {
        private HashSet<string> _propertyInfos;
        private Stack<KeyValuePair<string, Func<string, QueryOperation>>> _executableQueryExpressions;

        /// <summary>
        /// ExecutableEnumerator returns with the top level element of the Stack which was added via AddProcess in order to execute them in an intended order
        /// </summary>
        public IEnumerator<KeyValuePair<string, Func<string, QueryOperation>>> ExecutableEnumerator => _executableQueryExpressions.GetEnumerator();

        /// <summary>
        /// Type which contains the Properties which the rules inteded to apply for
        /// </summary>
        public Type TargetType { get; private set; }

        /// <summary>
        /// QueryParser contains the dev designed filter based behaviours
        /// </summary>
        /// <param name="targetType"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public QueryParser(Type targetType)
        {
            if (targetType is null)
            {
                throw new ArgumentNullException(nameof(targetType));
            }

            TargetType = targetType;
            _executableQueryExpressions = new Stack<KeyValuePair<string, Func<string, QueryOperation>>>();

            var properties = TargetType.GetProperties().Select(a => a.Name).ToHashSet();
            _propertyInfos = new HashSet<string>(properties, StringComparer.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Add item to the top of the Stack. The expressions will be executed in LIFO order.
        /// </summary>
        /// <param name="queryFilterCondition">The Regex expression which descibes how query parameter can be uniquely identified</param>
        /// <param name="ruleForFilteredItem">How should Query operation assembled based on query queryFilterCondition</param>
        public void AddProcess(string queryFilterCondition, Func<string, QueryOperation> ruleForFilteredItem)
        {
            var topElement = new KeyValuePair<string, Func<string, QueryOperation>>(queryFilterCondition, ruleForFilteredItem);
            _executableQueryExpressions.Push(topElement);
        }












        private static string CreateStringFromCharacterUntilIndexFromEnd(string input, char fromCharacter, int characterFromEnd = 0)
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

        private static string CreateStringFromIndexUntilEndCharacter(string input, char untilCharacter, int startIndex = 0)
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

        private string GetOriginalPropertyName(string filteredPropName, string value, int numberValue)
        {
            string foundProp = null;
            if (!_propertyInfos.TryGetValue(filteredPropName, out foundProp) || !int.TryParse(value, out numberValue))
            {
                throw new Exception();
            }

            return foundProp;
        }

        private void FillProcesses()
        {
            var example1 = "endYear[gte]=10";
            var kp1 = new KeyValuePair<string, Func<string, QueryOperation>>(@"(\[gte\])",
            delegate (string userDefinedSingleQuery)
            {
                var val = CreateStringFromCharacterUntilIndexFromEnd(userDefinedSingleQuery, '=', 0);
                var propName = CreateStringFromIndexUntilEndCharacter(userDefinedSingleQuery, '[', 0);
                var filteredPropName = FilterProperties(propName);
                var num = GetIntegerFromValue(val);
                var foundProp = GetOriginalPropertyName(filteredPropName, val, num);
                var operation = DetermineOperation(userDefinedSingleQuery);
                var task = SetFilterLinqQuery(num, foundProp, operation);

                return new QueryOperation { Value = val, PropertyName = propName, Task = task };
            });

            var example2 = "sort_by=desc(start_year)";
            var kp2 = new KeyValuePair<string, Func<string, QueryOperation>>(@"sort_by=desc(\(.*?\))",
            delegate (string split)
                {
                    var val = split.Substring(0, split.IndexOf("="));
                    var propName = split.Substring((split.IndexOf("(") + 1), split.Length - split.IndexOf("(") - 2);

                    var pattern = @"(\s+|@|&|'|\(|\)|<|>|#|_)";
                    var filteredPropName = Regex.Replace(propName, pattern, string.Empty).ToLower();

                    if (!_propertyInfos.Contains(filteredPropName))
                    {
                        return null;
                    }

                    return new QueryOperation { Value = val, PropertyName = filteredPropName, Task = null };
                });
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

        private Func<int, int, bool> DetermineOperation(string userDefinedSingleQuery)
        {
            Func<int, int, bool> operation = null;

            if (userDefinedSingleQuery.Contains("gte"))
            {
                operation = Greater<int>();
            }
            else if (userDefinedSingleQuery.Contains("lte"))
            {
                operation = Less<int>();
            }
            else if (userDefinedSingleQuery.Contains("eq"))
            {
                operation = Equal<int>();
            }
            else if (userDefinedSingleQuery.Contains("desc"))
            {

            }
            else if (userDefinedSingleQuery.Contains("asc"))
            {

            }

            return operation;
        }
    }
}
