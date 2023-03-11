namespace StarWars.JediArchives.Infrastructure.QueryParser
{
    public class QueryProcessorService<T> : IQueryProcessorService<T> where T : class
    {
        IServiceCollection _services;
        public QueryProcessorService(IServiceCollection services) 
        { 
            _services = services;
        }

        public void AddQueryProcessor(Func<Type, IQueryProcessorBuilder> queryProcessorBuilder)
        {
            var invoked = queryProcessorBuilder.Invoke(typeof(T));
            var builder = (IBuilder<QueryProcessor>)invoked;
            var queryProcessor = builder.Build();
            
            _services.Add(new ServiceDescriptor(typeof(IQueryProcessor), queryProcessor));
        }
    }
}
