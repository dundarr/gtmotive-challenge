namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.CreateCar
{
    /// <summary>
    /// Output for the CreateCar use case.
    /// </summary>
    public class CreateCarOutput : IUseCaseOutput
    {
        /// <summary>
        /// Gets or sets the id of the created car.
        /// </summary>
        public string Id { get; set; }
    }
}
