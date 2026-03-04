using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.CreateCar
{
    /// <summary>
    /// Use case for creating a new car.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CreateCarUseCase"/> class.
    /// </remarks>
    /// <param name="carRepository">The car repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="outputPort">The output port.</param>
    public class CreateCarUseCase(
        ICarRepository carRepository,
        IUnitOfWork unitOfWork,
        IOutputPortStandard<CreateCarOutput> outputPort) : IUseCase<CreateCarInput>
    {
        private const int MaxManufactureAgeYears = 5;

        private readonly ICarRepository _carRepository = carRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IOutputPortStandard<CreateCarOutput> _outputPort = outputPort;

        /// <inheritdoc/>
        public async Task Execute(CreateCarInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            var minDateOfManufacture = DateOnly.FromDateTime(DateTime.UtcNow).AddYears(-MaxManufactureAgeYears);
            if (input.DateOfManufacture < minDateOfManufacture)
            {
                throw new DomainException(
                    $"Date of manufacture must be within the last {MaxManufactureAgeYears} years. The earliest allowed date is {minDateOfManufacture:yyyy-MM-dd}.");
            }

            var car = new Car
            {
                PlateNumber = input.PlateNumber,
                Model = input.Model,
                DateOfManufacture = input.DateOfManufacture,
                KilometersRun = input.KilometersRun,
            };

            await _carRepository.AddAsync(car);
            await _unitOfWork.Save();

            _outputPort.StandardHandle(new CreateCarOutput { Id = car.Id });
        }
    }
}
