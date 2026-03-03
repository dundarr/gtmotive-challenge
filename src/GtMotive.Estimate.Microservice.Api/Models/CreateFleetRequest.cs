using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GtMotive.Estimate.Microservice.Api.Models
{
    /// <summary>
    /// Request model for creating a fleet vehicle.
    /// </summary>
    public class CreateFleetRequest
    {
        /// <summary>
        /// Gets or sets the plate number.
        /// </summary>
        [Required]
        public string PlateNumber { get; set; }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        [Required]
        public string Model { get; set; }

        /// <summary>
        /// Gets or sets the date of manufacture.
        /// </summary>
        [Required]
        [JsonRequired]
        public DateOnly DateOfManufacture { get; set; }

        /// <summary>
        /// Gets or sets the kilometers run.
        /// </summary>
        [Required]
        [Range(0, int.MaxValue)]
        public int KilometersRun { get; set; }
    }
}
