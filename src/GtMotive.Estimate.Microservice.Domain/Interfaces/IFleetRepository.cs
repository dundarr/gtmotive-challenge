// <copyright file="IFleetRepository.cs" company="GtMotive">
//   Copyright (c) GtMotive. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Entities;

namespace GtMotive.Estimate.Microservice.Domain.Interfaces
{
    /// <summary>
    /// Repository for fleet persistence.
    /// </summary>
    public interface IFleetRepository
    {
        /// <summary>
        /// Adds a new fleet vehicle.
        /// </summary>
        /// <param name="fleet">The fleet to add.</param>
        /// <returns>A task that completes when the fleet is stored.</returns>
        Task AddAsync(Fleet fleet);

        /// <summary>
        /// Gets a fleet vehicle by its identifier.
        /// </summary>
        /// <param name="fleetId">The fleet identifier.</param>
        /// <returns>The fleet, or null if not found.</returns>
        Task<Fleet> GetByIdAsync(string fleetId);

        /// <summary>
        /// Gets all fleet vehicles stored in the database.
        /// </summary>
        /// <returns>All fleet vehicles.</returns>
        Task<IReadOnlyList<Fleet>> GetAllAsync();
    }
}
