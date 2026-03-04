namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.ReturnCar
{
    /// <summary>
    /// Input for the ReturnCar use case.
    /// </summary>
    public class ReturnCarInput : IUseCaseInput
    {
        /// <summary>
        /// Gets or sets the car identifier to return. Only the rent status for this car is updated.
        /// Any authenticated user can return any rented vehicle.
        /// </summary>
        public string CarId { get; set; }
    }
}
