namespace StarWars.JediArchives.Infrastructure.QueryParser
{
    public static class QueryProcessorExtensions
    { 
        public static void AddQueryProcessor<T>(this IServiceCollection serviceProvider, Func<Type, IQueryProcessorBuilder> queryProcessorBuilder) where T : class
        {
            var invoked = queryProcessorBuilder.Invoke(typeof(T));
            var builder = (IBuilder<QueryProcessor>)invoked;
            var queryProcessor = builder.Build();

            serviceProvider.Add(new ServiceDescriptor(typeof(IQueryProcessor), queryProcessor));
        }
    }
}
