using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Entities;

namespace GtMotive.Estimate.Microservice.Domain.Interfaces
{
    /// <summary>
    /// Repository for rental persistence.
    /// </summary>
    public interface IRentalRepository
    {
        /// <summary>
        /// Gets the list of fleet identifiers that are currently rented (have an active rental).
        /// </summary>
        /// <returns>The list of rented fleet ids.</returns>
        Task<IReadOnlyList<string>> GetActiveRentedFleetIdsAsync();

        /// <summary>
        /// Adds a new rental.
        /// </summary>
        /// <param name="rental">The rental to add.</param>
        /// <returns>A task that completes when the rental is stored.</returns>
        Task AddAsync(Rental rental);

        /// <summary>
        /// Gets the active (not returned) rental for the given user, if any.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The active rental or null.</returns>
        Task<Rental> GetActiveByUserIdAsync(string userId);

        /// <summary>
        /// Gets the active (not returned) rental for the given fleet vehicle, if any.
        /// </summary>
        /// <param name="fleetId">The fleet identifier.</param>
        /// <returns>The active rental or null.</returns>
        Task<Rental> GetActiveByFleetIdAsync(string fleetId);

        /// <summary>
        /// Updates an existing rental.
        /// </summary>
        /// <param name="rental">The rental to update.</param>
        /// <returns>A task that completes when the rental is updated.</returns>
        Task UpdateAsync(Rental rental);
    }
}
