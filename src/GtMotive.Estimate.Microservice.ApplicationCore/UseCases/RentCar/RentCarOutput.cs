namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.RentCar
{
    /// <summary>
    /// Output for the RentCar use case.
    /// </summary>
    public class RentCarOutput : IUseCaseOutput
    {
        /// <summary>
        /// Gets or sets the id of the created rental.
        /// </summary>
        public string RentalId { get; set; }
    }
}
