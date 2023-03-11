namespace StarWars.JediArchives.Application.Contracts.Infrastructure
{
    /// <summary>
    /// This is the highest layer of incomping complex Query handling
    /// </summary>
    public interface IQueryParserService<T> where T : class
    {
        void UseQueryParser(Func<Type, IQueryProcessorBuilder> queryProcessorBuilder);
    }
}
