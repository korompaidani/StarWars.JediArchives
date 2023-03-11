namespace StarWars.JediArchives.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
#if RELEASE
            services.AddDbContext<StarWarsJediArchivesDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("StarWarsJediArchivesSqlServerConnectionString")));
#endif
#if DEBUG
            services.AddDbContext<StarWarsJediArchivesDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("StarWarsJediArchivesLocalDbConnectionString"),
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();
                }
                ));
#endif
            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITimelineRepository, TimelineRepository>();

            return services;
        }
    }
}
