namespace StarWars.JediArchives.Infrastructure.QueryParser
{
    public class QueryParserService<T> : IQueryParserService<T> where T : class
    {
        IServiceCollection _services;
        public QueryParserService(IServiceCollection services) 
        { 
            _services = services;
        }

        public void UseQueryParser(Func<Type, IQueryProcessorBuilder> queryProcessorBuilder)
        {
            var invoked = queryProcessorBuilder.Invoke(typeof(T));
            var builder = (IBuilder<QueryProcessor>)invoked;
            var queryProcessor = builder.Build();
            
            _services.Add(new ServiceDescriptor(typeof(IQueryPropcessor), queryProcessor));
        }
    }
}
