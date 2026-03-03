namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.ReturnFleet
{
    /// <summary>
    /// Input for the ReturnFleet use case.
    /// </summary>
    public class ReturnFleetInput : IUseCaseInput
    {
        /// <summary>
        /// Gets or sets the fleet vehicle identifier to return. Only the rent status for this fleet car is updated.
        /// Any authenticated user can return any rented vehicle.
        /// </summary>
        public string FleetId { get; set; }
    }
}
