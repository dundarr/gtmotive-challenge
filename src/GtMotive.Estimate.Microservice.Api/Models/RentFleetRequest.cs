using System.ComponentModel.DataAnnotations;

namespace GtMotive.Estimate.Microservice.Api.Models
{
    /// <summary>
    /// Request model for renting a fleet vehicle.
    /// </summary>
    public class RentFleetRequest
    {
        /// <summary>
        /// Gets or sets the fleet vehicle identifier to rent.
        /// </summary>
        [Required]
        public string FleetId { get; set; }
    }
}
