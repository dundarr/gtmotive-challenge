// <copyright file="ICarRepository.cs" company="GtMotive">
//   Copyright (c) GtMotive. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Entities;

namespace GtMotive.Estimate.Microservice.Domain.Interfaces
{
    /// <summary>
    /// Repository for car persistence.
    /// </summary>
    public interface ICarRepository
    {
        /// <summary>
        /// Adds a new car.
        /// </summary>
        /// <param name="car">The car to add.</param>
        /// <returns>A task that completes when the car is stored.</returns>
        Task AddAsync(Car car);

        /// <summary>
        /// Gets a car by its identifier.
        /// </summary>
        /// <param name="carId">The car identifier.</param>
        /// <returns>The car, or null if not found.</returns>
        Task<Car> GetByIdAsync(string carId);

        /// <summary>
        /// Gets all cars stored in the database.
        /// </summary>
        /// <returns>All cars.</returns>
        Task<IReadOnlyList<Car>> GetAllAsync();
    }
}
