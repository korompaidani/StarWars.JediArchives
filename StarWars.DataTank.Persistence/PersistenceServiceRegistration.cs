using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StarWars.DataTank.Application.Contracts.Persistence;
using StarWars.DataTank.Persistence.Repositories;

namespace StarWars.DataTank.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<StarWarsJediArchivesDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("StarWarsJediArchivesConnectionString")));

            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITimelineRepository, TimelineRepository>();

            return services;
        }
    }
}
