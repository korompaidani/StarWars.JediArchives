namespace StarWars.JediArchives.Application.Contracts.Infrastructure
{
    public interface IQueryPropcessor
    {
        /// <summary>
        /// It Should be call at Linq qery executions (3)
        /// ExecutableEnumerator returns with the top level element of the Stack which was added via AddProcess in order to execute them in an intended order
        /// </summary>
        IEnumerator<QueryOperation> ExecutableEnumerator { get; }

        /// <summary>
        /// Type which contains the Properties which the rules inteded to apply for
        /// </summary>
        Type TargetType { get; init; }

        /// <summary>
        /// It Should be call at Setup time via its Builder in order to define rules (1)
        /// Add item to the top of the Stack. The expressions will be executed in LIFO order.
        /// </summary>
        /// <param name="queryFilterCondition">The Regex expression which descibes how query parameter can be uniquely identified</param>
        /// <param name="ruleForFilteredItem">How should Query operation assembled based on query queryFilterCondition</param>
        void AddProcess(string queryFilterCondition, Func<string, QueryOperation> ruleForFilteredItem);

        /// <summary>
        /// It Should be call when the incoming aggregated Query processing intended (2)
        /// </summary>
        /// <param name="incomingAggregatedParameter">This contains all user defined query parameters in one string</param>
        void Run(string incomingAggregatedParameter);
    }
}
