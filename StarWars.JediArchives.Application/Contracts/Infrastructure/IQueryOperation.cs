using System;

namespace StarWars.JediArchives.Application.Contracts.Infrastructure
{
    public interface IQueryOperation
    {
        /// <summary>
        /// Contains the value of the query
        /// </summary>
        string Value { get; set; }

        /// <summary>
        /// Contains the name of affected property name of dto
        /// </summary>
        string PropertyName { get; set; }

        /// <summary>
        /// Task contains the delegate query for Linq which should examine each item of a collection and returns a boolean
        /// </summary>
        Func<dynamic, bool> Task { get; set; }
    }
}