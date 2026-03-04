// <copyright file="Rental.cs" company="GtMotive">
//   Copyright (c) GtMotive. All rights reserved.
// </copyright>

using System;

namespace GtMotive.Estimate.Microservice.Domain.Entities
{
    /// <summary>
    /// Represents a car rental in the domain.
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
        /// Gets or sets the rented car identifier.
        /// </summary>
        public string CarId { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the car was rented.
        /// </summary>
        public DateTime RentedAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the car was returned; null if still rented.
        /// </summary>
        public DateTime? ReturnedAt { get; set; }
    }
}
