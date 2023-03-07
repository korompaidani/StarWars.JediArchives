using StarWars.JediArchives.Application.Contracts.Infrastructure;
using System;

namespace StarWars.JediArchives.Infrastructure.QueryParser
{
    public class QueryOperation : IQueryOperation
    {
        public string Value { get; set; }
        public string PropertyName { get; set; }
        public Func<dynamic, bool> Task { get; set; }
    }
}
