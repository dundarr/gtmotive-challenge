using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetAvailableCars
{
    /// <summary>
    /// Represents an available car in the GetAvailableCars output.
    /// </summary>
    public class AvailableCarOutputItem
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

        /// <summary>
        /// Gets or sets the rent status (e.g. "Available").
        /// </summary>
        public string RentStatus { get; set; }
    }
}
