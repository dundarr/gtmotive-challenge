using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetAvailableFleets
{
    /// <summary>
    /// Use case for getting the list of fleet vehicles that are available (not in rented status).
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="GetAvailableFleetsUseCase"/> class.
    /// </remarks>
    /// <param name="fleetRepository">The fleet repository.</param>
    /// <param name="rentalRepository">The rental repository.</param>
    /// <param name="outputPort">The output port.</param>
    public class GetAvailableFleetsUseCase(
        IFleetRepository fleetRepository,
        IRentalRepository rentalRepository,
        IOutputPortStandard<GetAvailableFleetsOutput> outputPort) : IUseCase<GetAvailableFleetsInput>
    {
        private const string AvailableStatus = "Available";

        private readonly IFleetRepository _fleetRepository = fleetRepository;
        private readonly IRentalRepository _rentalRepository = rentalRepository;
        private readonly IOutputPortStandard<GetAvailableFleetsOutput> _outputPort = outputPort;

        /// <inheritdoc/>
        public async Task Execute(GetAvailableFleetsInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            var allFleets = await _fleetRepository.GetAllAsync();
            var rentedFleetIds = await _rentalRepository.GetActiveRentedFleetIdsAsync();
            var rentedSet = new HashSet<string>(rentedFleetIds, StringComparer.Ordinal);

            var availableFleets = allFleets
                .Where(fleet => !rentedSet.Contains(fleet.Id))
                .Select(fleet => new AvailableFleetOutputItem
                {
                    Id = fleet.Id,
                    PlateNumber = fleet.PlateNumber,
                    Model = fleet.Model,
                    DateOfManufacture = fleet.DateOfManufacture,
                    KilometersRun = fleet.KilometersRun,
                    RentStatus = AvailableStatus,
                })
                .ToList();

            _outputPort.StandardHandle(new GetAvailableFleetsOutput { Fleets = availableFleets });
        }
    }
}
