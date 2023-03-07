using System;
using System.Collections.Generic;
using System.Linq;

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
    }
}
