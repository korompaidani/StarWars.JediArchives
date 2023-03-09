﻿namespace StarWars.JediArchives.Infrastructure.QueryParser
{
    public class QueryProcessor : IQueryPropcessor
    {
        private HashSet<string> _propertyInfos;
        private Stack<QueryOperation> _executableQueryExpressions;

        private List<KeyValuePair<string, Func<string, QueryOperation>>> _processes;
        private List<KeyValuePair<string, Func<string, QueryOperation>>> Processes => _processes.ToList();

        public IEnumerator<QueryOperation> ExecutableEnumerator => _executableQueryExpressions.GetEnumerator();

        public Type TargetType { get; init; }

        /// <summary>
        /// QueryParser contains the dev designed filter based behaviours
        /// </summary>
        /// <param name="targetType"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public QueryProcessor(Type targetType)
        {
            if (targetType is null)
            {
                throw new ArgumentNullException(nameof(targetType));
            }

            TargetType = targetType;
            _executableQueryExpressions = new Stack<QueryOperation>();
            
            var propertyCollection = TargetType.GetProperties().Select(a => a.Name).ToHashSet();            
            _propertyInfos = new HashSet<string>(propertyCollection, StringComparer.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// QueryParser contains the dev designed filter based behaviours
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="propertyCollection"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public QueryProcessor(Type targetType, HashSet<string> propertyCollection)
        {
            if (targetType is null)
            {
                throw new ArgumentNullException(nameof(targetType));
            }
            if (propertyCollection is null)
            {
                throw new ArgumentNullException(nameof(propertyCollection));
            }

            TargetType = targetType;
            _propertyInfos = propertyCollection;
            _processes = new List<KeyValuePair<string, Func<string, QueryOperation>>>();
            _executableQueryExpressions = new Stack<QueryOperation>();
        }

        public void AddProcess(string queryFilterCondition, Func<string, QueryOperation> ruleForFilteredItem)
        {
            var process = new KeyValuePair<string, Func<string, QueryOperation>>(queryFilterCondition, ruleForFilteredItem);
            _processes.Add(process);
        }

        public void Run(string incomingAggregatedParameter)
        {
            var incomingParameters = incomingAggregatedParameter.Split('&');
            var _oneRunLifeList = Processes;

            foreach (var item in incomingParameters)
            {
                KeyValuePair<string, Func<string, QueryOperation>>? foundKey = null;

                foreach (var proc in _oneRunLifeList)
                {
                    var res = Regex.Matches(item, proc.Key);
                    if (res.Count > 0)
                    {
                        var operation = proc.Value(item);
                        foundKey = proc;
                        if (operation != null)
                        {
                            _executableQueryExpressions.Push(operation);
                            break;
                        }
                    }
                }

                if (foundKey != null)
                {
                    _oneRunLifeList.Remove(foundKey.Value);
                }
            }

        }
    }
}