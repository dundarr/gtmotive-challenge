namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.RentFleet
{
    /// <summary>
    /// Input for the RentFleet use case.
    /// </summary>
    public class RentFleetInput : IUseCaseInput
    {
        /// <summary>
        /// Gets or sets the user/person identifier (from JWT).
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the fleet vehicle identifier to rent.
        /// </summary>
        public string FleetId { get; set; }
    }
}
