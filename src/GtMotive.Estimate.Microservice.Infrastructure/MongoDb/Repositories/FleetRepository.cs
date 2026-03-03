using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Documents;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Repositories
{
    /// <summary>
    /// MongoDB implementation of <see cref="IFleetRepository"/>.
    /// </summary>
    public class FleetRepository : IFleetRepository
    {
        private const string CollectionName = "Fleets";
        private readonly IMongoCollection<FleetDocument> _collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="FleetRepository"/> class.
        /// </summary>
        /// <param name="mongoService">The MongoDB service.</param>
        /// <param name="options">The MongoDB settings.</param>
        public FleetRepository(MongoService mongoService, IOptions<MongoDbSettings> options)
        {
            ArgumentNullException.ThrowIfNull(mongoService);
            ArgumentNullException.ThrowIfNull(options);

            var database = mongoService.MongoClient.GetDatabase(options.Value.MongoDbDatabaseName);
            _collection = database.GetCollection<FleetDocument>(CollectionName);
        }

        /// <inheritdoc/>
        public async Task AddAsync(Fleet fleet)
        {
            ArgumentNullException.ThrowIfNull(fleet);

            var document = new FleetDocument
            {
                PlateNumber = fleet.PlateNumber,
                Model = fleet.Model,
                DateOfManufacture = fleet.DateOfManufacture.ToDateTime(TimeOnly.MinValue),
                KilometersRun = fleet.KilometersRun,
            };

            await _collection.InsertOneAsync(document);

            fleet.Id = document.Id;
        }

        /// <inheritdoc/>
        public async Task<Fleet> GetByIdAsync(string fleetId)
        {
            var document = await _collection.Find(d => d.Id == fleetId).FirstOrDefaultAsync();
            return document == null ? null : MapToFleet(document);
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<Fleet>> GetAllAsync()
        {
            var documents = await _collection.Find(FilterDefinition<FleetDocument>.Empty).ToListAsync();
#pragma warning disable IDE0305 // Simplify collection initialization
            return documents.Select(MapToFleet).ToList();
#pragma warning restore IDE0305
        }

        private static Fleet MapToFleet(FleetDocument doc)
        {
            return new Fleet
            {
                Id = doc.Id,
                PlateNumber = doc.PlateNumber,
                Model = doc.Model,
                DateOfManufacture = DateOnly.FromDateTime(doc.DateOfManufacture),
                KilometersRun = doc.KilometersRun,
            };
        }
    }
}
