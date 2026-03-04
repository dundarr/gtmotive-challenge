namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.RentCar
{
    /// <summary>
    /// Input for the RentCar use case.
    /// </summary>
    public class RentCarInput : IUseCaseInput
    {
        /// <summary>
        /// Gets or sets the user/person identifier (from JWT).
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the car identifier to rent.
        /// </summary>
        public string CarId { get; set; }
    }
}
