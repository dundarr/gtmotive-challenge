namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.CreateFleet
{
    /// <summary>
    /// Output for the CreateFleet use case.
    /// </summary>
    public class CreateFleetOutput : IUseCaseOutput
    {
        /// <summary>
        /// Gets or sets the id of the created fleet vehicle.
        /// </summary>
        public string Id { get; set; }
    }
}
