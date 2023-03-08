namespace StarWars.JediArchives.Infrastructure.QueryParser
{
    public record QueryOperation : IQueryOperation
    {
        public string Value { get; set; }
        public string PropertyName { get; set; }
        public Func<dynamic, bool> Task { get; set; }
    }
}
