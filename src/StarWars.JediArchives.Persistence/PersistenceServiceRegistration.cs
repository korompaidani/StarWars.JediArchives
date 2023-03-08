﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StarWars.JediArchives.Application.Contracts.Persistence;
using StarWars.JediArchives.Persistence.Repositories;

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
                options.UseSqlServer(configuration.GetConnectionString("StarWarsJediArchivesLocalDbConnectionString")));
#endif
            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITimelineRepository, TimelineRepository>();

            return services;
        }
    }
}