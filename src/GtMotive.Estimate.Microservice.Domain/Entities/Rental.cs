using System;

namespace GtMotive.Estimate.Microservice.Domain.Entities
{
    /// <summary>
    /// Represents a fleet rental in the domain.
    /// </summary>
    public class Rental
    {
        /// <summary>
        /// Gets or sets the unique identifier (set by persistence).
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the user/person identifier (from JWT).
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the rented fleet vehicle identifier.
        /// </summary>
        public string FleetId { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the fleet vehicle was rented.
        /// </summary>
        public DateTime RentedAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the fleet vehicle was returned; null if still rented.
        /// </summary>
        public DateTime? ReturnedAt { get; set; }
    }
}
