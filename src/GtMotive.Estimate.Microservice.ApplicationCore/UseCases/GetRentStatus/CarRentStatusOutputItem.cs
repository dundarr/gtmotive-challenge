using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetRentStatus
{
    /// <summary>
    /// Represents a car with its rent status in the GetRentStatus output.
    /// </summary>
    public class CarRentStatusOutputItem
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
        /// Gets or sets the rent status (e.g. "Available" or "Rented").
        /// </summary>
        public string RentStatus { get; set; }
    }
}
