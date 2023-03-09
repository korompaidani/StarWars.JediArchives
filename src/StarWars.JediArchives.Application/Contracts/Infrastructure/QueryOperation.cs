namespace StarWars.JediArchives.Infrastructure.QueryParser
{
    public record QueryOperation
    {
        /// <summary>
        /// Contains the value of the query
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Contains the name of affected property name of dto
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Task contains the delegate query for Linq which should examine each item of a collection and returns a boolean
        /// </summary>
        public Func<dynamic, bool> CompareTask { get; set; }

        /// <summary>
        /// Task contains the delegate query for Linq which should examine orderBy
        /// </summary>
        public Func<dynamic, dynamic> OrderByTask { get; set; }
    }
}
