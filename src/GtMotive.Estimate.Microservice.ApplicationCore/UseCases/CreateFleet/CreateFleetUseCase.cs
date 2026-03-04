using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.CreateFleet
{
    /// <summary>
    /// Use case for creating a new fleet vehicle.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CreateFleetUseCase"/> class.
    /// </remarks>
    /// <param name="fleetRepository">The fleet repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="outputPort">The output port.</param>
    public class CreateFleetUseCase(
        IFleetRepository fleetRepository,
        IUnitOfWork unitOfWork,
        IOutputPortStandard<CreateFleetOutput> outputPort) : IUseCase<CreateFleetInput>
    {
        private const int MaxManufactureAgeYears = 5;

        private readonly IFleetRepository _fleetRepository = fleetRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IOutputPortStandard<CreateFleetOutput> _outputPort = outputPort;

        /// <inheritdoc/>
        public async Task Execute(CreateFleetInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            var minDateOfManufacture = DateOnly.FromDateTime(DateTime.UtcNow).AddYears(-MaxManufactureAgeYears);
            if (input.DateOfManufacture < minDateOfManufacture)
            {
                throw new DomainException(
                    $"Date of manufacture must be within the last {MaxManufactureAgeYears} years. The earliest allowed date is {minDateOfManufacture:yyyy-MM-dd}.");
            }

            var fleet = new Fleet
            {
                PlateNumber = input.PlateNumber,
                Model = input.Model,
                DateOfManufacture = input.DateOfManufacture,
                KilometersRun = input.KilometersRun,
            };

            await _fleetRepository.AddAsync(fleet);
            await _unitOfWork.Save();

            _outputPort.StandardHandle(new CreateFleetOutput { Id = fleet.Id });
        }
    }
}
