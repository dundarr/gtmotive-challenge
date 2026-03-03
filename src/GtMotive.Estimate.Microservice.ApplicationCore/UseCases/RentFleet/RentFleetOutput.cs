namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.RentFleet
{
    /// <summary>
    /// Output for the RentFleet use case.
    /// </summary>
    public class RentFleetOutput : IUseCaseOutput
    {
        /// <summary>
        /// Gets or sets the id of the created rental.
        /// </summary>
        public string RentalId { get; set; }
    }
}
