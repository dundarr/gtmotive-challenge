namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.ReturnFleet
{
    /// <summary>
    /// Output for the ReturnFleet use case.
    /// </summary>
    public class ReturnFleetOutput : IUseCaseOutput
    {
        /// <summary>
        /// Gets or sets the id of the rental that was returned.
        /// </summary>
        public string RentalId { get; set; }
    }
}
