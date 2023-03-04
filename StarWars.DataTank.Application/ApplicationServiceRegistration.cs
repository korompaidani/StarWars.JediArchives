using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using StarWars.DataTank.Application.Contracts.Persistence;
using System.Reflection;

namespace StarWars.DataTank.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
