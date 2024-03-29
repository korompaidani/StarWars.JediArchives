﻿namespace StarWars.JediArchives.Infrastructure.QueryParser
{
    public static class QueryProcessorExtensions
    {
        public static void AddQueryProcessor<T>(this IServiceCollection serviceProvider, Func<Type, IQueryProcessorBuilder> queryProcessorBuilder) where T : class
        {
            var invoked = queryProcessorBuilder.Invoke(typeof(T));
            var builder = (IBuilder<QueryProcessor<T>>)invoked;
            var queryProcessor = builder.Build();
            serviceProvider.Add(new ServiceDescriptor(typeof(IQueryProcessor<T>), queryProcessor));
        }

        public static void AddQueryProcessor<T>(this IServiceCollection serviceProvider, Action<QueryProcessorStatedBuilder<T>> queryProcessorBuilder) where T : class
        {
            QueryProcessorStatedBuilder<T> bld = new QueryProcessorStatedBuilder<T>(typeof(T));
            queryProcessorBuilder.Invoke(bld);
            object queryProcessor = ((IBuilder<QueryProcessor<T>>)bld).Build();
            serviceProvider.Add(new ServiceDescriptor(typeof(IQueryProcessor<T>), queryProcessor));
        }
    }
}