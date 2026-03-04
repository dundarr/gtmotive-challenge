using System;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetCars
{
    /// <summary>
    /// Use case for getting the list of cars stored in the database.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="GetCarsUseCase"/> class.
    /// </remarks>
    /// <param name="carRepository">The car repository.</param>
    /// <param name="outputPort">The output port.</param>
    public class GetCarsUseCase(
        ICarRepository carRepository,
        IOutputPortStandard<GetCarsOutput> outputPort) : IUseCase<GetCarsInput>
    {
        private readonly ICarRepository _carRepository = carRepository;
        private readonly IOutputPortStandard<GetCarsOutput> _outputPort = outputPort;

        /// <inheritdoc/>
        public async Task Execute(GetCarsInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            var cars = await _carRepository.GetAllAsync();
            var output = new GetCarsOutput
            {
#pragma warning disable IDE0305 // Simplify collection initialization
                Cars = cars.Select(c => new CarOutputItem
                {
                    Id = c.Id,
                    PlateNumber = c.PlateNumber,
                    Model = c.Model,
                    DateOfManufacture = c.DateOfManufacture,
                    KilometersRun = c.KilometersRun,
                }).ToList(),
#pragma warning restore IDE0305
            };

            _outputPort.StandardHandle(output);
        }
    }
}
