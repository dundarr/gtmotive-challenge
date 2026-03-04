// <copyright file="Car.cs" company="GtMotive">
//   Copyright (c) GtMotive. All rights reserved.
// </copyright>

using System;

namespace GtMotive.Estimate.Microservice.Domain.Entities
{
    /// <summary>
    /// Represents a car in the domain.
    /// </summary>
    public class Car
    {
        /// <summary>
        /// Gets or sets the unique identifier (set by persistence).
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the plate number.
        /// </summary>
        public string PlateNumber { get; set; }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Gets or sets the date of manufacture.
        /// </summary>
        public DateOnly DateOfManufacture { get; set; }

        /// <summary>
        /// Gets or sets the kilometers run.
        /// </summary>
        public int KilometersRun { get; set; }
    }
}
