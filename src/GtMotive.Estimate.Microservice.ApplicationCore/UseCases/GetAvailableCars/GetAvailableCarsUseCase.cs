using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetAvailableCars
{
    /// <summary>
    /// Use case for getting the list of cars that are available (not in rented status).
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="GetAvailableCarsUseCase"/> class.
    /// </remarks>
    /// <param name="carRepository">The car repository.</param>
    /// <param name="rentalRepository">The rental repository.</param>
    /// <param name="outputPort">The output port.</param>
    public class GetAvailableCarsUseCase(
        ICarRepository carRepository,
        IRentalRepository rentalRepository,
        IOutputPortStandard<GetAvailableCarsOutput> outputPort) : IUseCase<GetAvailableCarsInput>
    {
        private const string AvailableStatus = "Available";

        private readonly ICarRepository _carRepository = carRepository;
        private readonly IRentalRepository _rentalRepository = rentalRepository;
        private readonly IOutputPortStandard<GetAvailableCarsOutput> _outputPort = outputPort;

        /// <inheritdoc/>
        public async Task Execute(GetAvailableCarsInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            var allCars = await _carRepository.GetAllAsync();
            var rentedCarIds = await _rentalRepository.GetActiveRentedCarIdsAsync();
            var rentedSet = new HashSet<string>(rentedCarIds, StringComparer.Ordinal);

            var availableCars = allCars
                .Where(car => !rentedSet.Contains(car.Id))
                .Select(car => new AvailableCarOutputItem
                {
                    Id = car.Id,
                    PlateNumber = car.PlateNumber,
                    Model = car.Model,
                    DateOfManufacture = car.DateOfManufacture,
                    KilometersRun = car.KilometersRun,
                    RentStatus = AvailableStatus,
                })
                .ToList();

            _outputPort.StandardHandle(new GetAvailableCarsOutput { Cars = availableCars });
        }
    }
}
