using System;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetFleets
{
    /// <summary>
    /// Use case for getting the list of fleet vehicles stored in the database.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="GetFleetsUseCase"/> class.
    /// </remarks>
    /// <param name="fleetRepository">The fleet repository.</param>
    /// <param name="outputPort">The output port.</param>
    public class GetFleetsUseCase(
        IFleetRepository fleetRepository,
        IOutputPortStandard<GetFleetsOutput> outputPort) : IUseCase<GetFleetsInput>
    {
        private readonly IFleetRepository _fleetRepository = fleetRepository;
        private readonly IOutputPortStandard<GetFleetsOutput> _outputPort = outputPort;

        /// <inheritdoc/>
        public async Task Execute(GetFleetsInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            var fleets = await _fleetRepository.GetAllAsync();
            var output = new GetFleetsOutput
            {
#pragma warning disable IDE0305 // Simplify collection initialization
                Fleets = fleets.Select(f => new FleetOutputItem
                {
                    Id = f.Id,
                    PlateNumber = f.PlateNumber,
                    Model = f.Model,
                    DateOfManufacture = f.DateOfManufacture,
                    KilometersRun = f.KilometersRun,
                }).ToList(),
#pragma warning restore IDE0305
            };

            _outputPort.StandardHandle(output);
        }
    }
}
