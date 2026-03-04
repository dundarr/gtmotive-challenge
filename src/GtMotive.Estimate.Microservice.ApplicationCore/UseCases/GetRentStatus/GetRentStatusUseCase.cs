using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetRentStatus
{
    /// <summary>
    /// Use case for getting all fleet vehicles with their car data and rent status.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="GetRentStatusUseCase"/> class.
    /// </remarks>
    /// <param name="fleetRepository">The fleet repository.</param>
    /// <param name="rentalRepository">The rental repository.</param>
    /// <param name="outputPort">The output port.</param>
    public class GetRentStatusUseCase(
        IFleetRepository fleetRepository,
        IRentalRepository rentalRepository,
        IOutputPortStandard<GetRentStatusOutput> outputPort) : IUseCase<GetRentStatusInput>
    {
        private const string AvailableStatus = "Available";
        private const string RentedStatus = "Rented";

        private readonly IFleetRepository _fleetRepository = fleetRepository;
        private readonly IRentalRepository _rentalRepository = rentalRepository;
        private readonly IOutputPortStandard<GetRentStatusOutput> _outputPort = outputPort;

        /// <inheritdoc/>
        public async Task Execute(GetRentStatusInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            var allFleets = await _fleetRepository.GetAllAsync();
            var rentedFleetIds = await _rentalRepository.GetActiveRentedFleetIdsAsync();
            var rentedSet = new HashSet<string>(rentedFleetIds, StringComparer.Ordinal);

            var fleetsWithStatus = allFleets
                .Select(fleet => new FleetRentStatusOutputItem
                {
                    Id = fleet.Id,
                    PlateNumber = fleet.PlateNumber,
                    Model = fleet.Model,
                    DateOfManufacture = fleet.DateOfManufacture,
                    KilometersRun = fleet.KilometersRun,
                    RentStatus = rentedSet.Contains(fleet.Id) ? RentedStatus : AvailableStatus,
                })
                .ToList();

            _outputPort.StandardHandle(new GetRentStatusOutput { Fleets = fleetsWithStatus });
        }
    }
}
