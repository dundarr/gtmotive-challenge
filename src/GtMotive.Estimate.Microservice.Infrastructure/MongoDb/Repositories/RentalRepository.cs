using System;
using System.Collections.Generic;
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
    /// MongoDB implementation of <see cref="IRentalRepository"/>.
    /// </summary>
    public class RentalRepository : IRentalRepository
    {
        private const string CollectionName = "Rentals";
        private readonly IMongoCollection<RentalDocument> _collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="RentalRepository"/> class.
        /// </summary>
        /// <param name="mongoService">The MongoDB service.</param>
        /// <param name="options">The MongoDB settings.</param>
        public RentalRepository(MongoService mongoService, IOptions<MongoDbSettings> options)
        {
            ArgumentNullException.ThrowIfNull(mongoService);
            ArgumentNullException.ThrowIfNull(options);

            var database = mongoService.MongoClient.GetDatabase(options.Value.MongoDbDatabaseName);
            _collection = database.GetCollection<RentalDocument>(CollectionName);
        }

        /// <inheritdoc/>
        public async Task AddAsync(Rental rental)
        {
            ArgumentNullException.ThrowIfNull(rental);

            var document = new RentalDocument
            {
                UserId = rental.UserId,
                CarId = rental.CarId,
                RentedAt = rental.RentedAt,
                ReturnedAt = rental.ReturnedAt,
            };

            await _collection.InsertOneAsync(document);

            rental.Id = document.Id;
        }

        /// <inheritdoc/>
        public async Task<Rental> GetActiveByUserIdAsync(string userId)
        {
            var document = await _collection
                .Find(d => d.UserId == userId && d.ReturnedAt == null)
                .FirstOrDefaultAsync();
            return document == null ? null : MapToRental(document);
        }

        /// <inheritdoc/>
        public async Task<Rental> GetActiveByCarIdAsync(string carId)
        {
            var document = await _collection
                .Find(d => d.CarId == carId && d.ReturnedAt == null)
                .FirstOrDefaultAsync();
            return document == null ? null : MapToRental(document);
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<string>> GetActiveRentedCarIdsAsync()
        {
            var filter = Builders<RentalDocument>.Filter.Eq(d => d.ReturnedAt, null);
            var carIds = await _collection
                .Find(filter)
                .Project(d => d.CarId)
                .ToListAsync();
            return carIds;
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(Rental rental)
        {
            ArgumentNullException.ThrowIfNull(rental);

            var filter = Builders<RentalDocument>.Filter.Eq(d => d.Id, rental.Id);
            var update = Builders<RentalDocument>.Update.Set(d => d.ReturnedAt, rental.ReturnedAt);
            await _collection.UpdateOneAsync(filter, update);
        }

        private static Rental MapToRental(RentalDocument doc)
        {
            return new Rental
            {
                Id = doc.Id,
                UserId = doc.UserId,
                CarId = doc.CarId,
                RentedAt = doc.RentedAt,
                ReturnedAt = doc.ReturnedAt,
            };
        }
    }
}
