using System;

namespace GtMotive.Estimate.Microservice.Api.Models
{
    /// <summary>
    /// Response model for a fleet vehicle.
    /// </summary>
    public class FleetResponse
    {
        /// <summary>
        /// Gets or sets the unique identifier.
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
