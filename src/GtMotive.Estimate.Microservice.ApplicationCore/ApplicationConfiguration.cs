using System;
using System.Diagnostics.CodeAnalysis;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.CreateFleet;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetAvailableFleets;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetFleets;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.RentFleet;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.ReturnFleet;
using Microsoft.Extensions.DependencyInjection;

[assembly: CLSCompliant(false)]

namespace GtMotive.Estimate.Microservice.ApplicationCore
{
    /// <summary>
    /// Adds Use Cases classes.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ApplicationConfiguration
    {
        /// <summary>
        /// Adds Use Cases to the ServiceCollection.
        /// </summary>
        /// <param name="services">Service Collection.</param>
        /// <returns>The modified instance.</returns>
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            services.AddScoped<IUseCase<CreateFleetInput>, CreateFleetUseCase>();
            services.AddScoped<IUseCase<GetAvailableFleetsInput>, GetAvailableFleetsUseCase>();
            services.AddScoped<IUseCase<GetFleetsInput>, GetFleetsUseCase>();
            services.AddScoped<IUseCase<RentFleetInput>, RentFleetUseCase>();
            services.AddScoped<IUseCase<ReturnFleetInput>, ReturnFleetUseCase>();
            return services;
        }
    }
}
