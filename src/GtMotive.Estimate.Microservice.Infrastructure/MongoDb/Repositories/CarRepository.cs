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
    /// MongoDB implementation of <see cref="ICarRepository"/>.
    /// </summary>
    public class CarRepository : ICarRepository
    {
        private const string CollectionName = "Cars";
        private readonly IMongoCollection<CarDocument> _collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="CarRepository"/> class.
        /// </summary>
        /// <param name="mongoService">The MongoDB service.</param>
        /// <param name="options">The MongoDB settings.</param>
        public CarRepository(MongoService mongoService, IOptions<MongoDbSettings> options)
        {
            ArgumentNullException.ThrowIfNull(mongoService);
            ArgumentNullException.ThrowIfNull(options);

            var database = mongoService.MongoClient.GetDatabase(options.Value.MongoDbDatabaseName);
            _collection = database.GetCollection<CarDocument>(CollectionName);
        }

        /// <inheritdoc/>
        public async Task AddAsync(Car car)
        {
            ArgumentNullException.ThrowIfNull(car);

            var document = new CarDocument
            {
                PlateNumber = car.PlateNumber,
                Model = car.Model,
                DateOfManufacture = car.DateOfManufacture.ToDateTime(TimeOnly.MinValue),
                KilometersRun = car.KilometersRun,
            };

            await _collection.InsertOneAsync(document);

            car.Id = document.Id;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<Car>> GetAllAsync()
        {
            var documents = await _collection.Find(FilterDefinition<CarDocument>.Empty).ToListAsync();
#pragma warning disable IDE0305 // Simplify collection initialization
            return documents.Select(MapToCar).ToList();
#pragma warning restore IDE0305
        }

        private static Car MapToCar(CarDocument doc)
        {
            return new Car
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
