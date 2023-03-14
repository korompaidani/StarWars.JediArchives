namespace StarWars.JediArchives.Application.Contracts.Infrastructure.Query
{
    /// <summary>
    /// This is the highest layer of incomping complex Query handling
    /// </summary>
    public interface IQueryProcessorService<T> where T : class
    {
        void AddQueryProcessor(Func<Type, IQueryProcessorBuilder> queryProcessorBuilder);
    }
}
