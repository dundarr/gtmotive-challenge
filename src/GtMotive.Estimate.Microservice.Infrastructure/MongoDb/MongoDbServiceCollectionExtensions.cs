using System;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Infrastructure.Interfaces;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDb
{
    /// <summary>
    /// Extension methods for registering MongoDB services.
    /// </summary>
    public static class MongoDbServiceCollectionExtensions
    {
        /// <summary>
        /// Registers MongoService, FleetRepository and UnitOfWork with the container.
        /// </summary>
        /// <param name="builder">Instance returned by AddBaseInfrastructure.</param>
        /// <returns>The same builder for chaining.</returns>
        public static IInfrastructureBuilder AddMongoDb(this IInfrastructureBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.Services.AddSingleton<MongoService>();
            builder.Services.AddScoped<IUnitOfWork, MongoUnitOfWork>();
            builder.Services.AddScoped<IFleetRepository, FleetRepository>();
            builder.Services.AddScoped<IRentalRepository, RentalRepository>();

            return builder;
        }
    }
}
