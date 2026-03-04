namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.ReturnCar
{
    /// <summary>
    /// Output for the ReturnCar use case.
    /// </summary>
    public class ReturnCarOutput : IUseCaseOutput
    {
        /// <summary>
        /// Gets or sets the id of the rental that was returned.
        /// </summary>
        public string RentalId { get; set; }
    }
}
