using System.ComponentModel.DataAnnotations;

namespace GtMotive.Estimate.Microservice.Api.Models
{
    /// <summary>
    /// Request model for renting a car.
    /// </summary>
    public class RentCarRequest
    {
        /// <summary>
        /// Gets or sets the car identifier to rent.
        /// </summary>
        [Required]
        public string CarId { get; set; }
    }
}
