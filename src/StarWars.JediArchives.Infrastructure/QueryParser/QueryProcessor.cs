namespace StarWars.JediArchives.Infrastructure.QueryParser
{
    public class QueryProcessor
    {
        private HashSet<string> _propertyInfos;
        private Stack<QueryOperation> _executableQueryExpressions;

        private List<KeyValuePair<string, Func<string, QueryOperation>>> _processes;
        private List<KeyValuePair<string, Func<string, QueryOperation>>> Processes => _processes.ToList();

        /// <summary>
        /// It Should be call at Linq qery executions (1)
        /// ExecutableEnumerator returns with the top level element of the Stack which was added via AddProcess in order to execute them in an intended order
        /// </summary>
        public IEnumerator<QueryOperation> ExecutableEnumerator => _executableQueryExpressions.GetEnumerator();

        /// <summary>
        /// Type which contains the Properties which the rules inteded to apply for
        /// </summary>
        public Type TargetType { get; private set; }

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
        /// <param name="propertyCollection"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public QueryProcessor(HashSet<string> propertyCollection)
        {
            if (propertyCollection is null)
            {
                throw new ArgumentNullException(nameof(propertyCollection));
            }

            _propertyInfos = propertyCollection;
            _processes = new List<KeyValuePair<string, Func<string, QueryOperation>>>();
            _executableQueryExpressions = new Stack<QueryOperation>();
        }

        /// <summary>
        /// It Should be call at Setup time via its Builder in order to define rules (3)
        /// Add item to the top of the Stack. The expressions will be executed in LIFO order.
        /// </summary>
        /// <param name="queryFilterCondition">The Regex expression which descibes how query parameter can be uniquely identified</param>
        /// <param name="ruleForFilteredItem">How should Query operation assembled based on query queryFilterCondition</param>
        public void AddProcess(string queryFilterCondition, Func<string, QueryOperation> ruleForFilteredItem)
        {
            var process = new KeyValuePair<string, Func<string, QueryOperation>>(queryFilterCondition, ruleForFilteredItem);
            _processes.Add(process);
        }

        /// <summary>
        /// It Should be call when the incoming aggregated Query processing intended (2)
        /// </summary>
        /// <param name="incomingAggregatedParameter">This contains all user defined query parameters in one string</param>
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