using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.RentCar
{
    /// <summary>
    /// Use case for renting a car. Only one active rental per user is allowed.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="RentCarUseCase"/> class.
    /// </remarks>
    /// <param name="carRepository">The car repository.</param>
    /// <param name="rentalRepository">The rental repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="outputPort">The output port.</param>
    public class RentCarUseCase(
        ICarRepository carRepository,
        IRentalRepository rentalRepository,
        IUnitOfWork unitOfWork,
        IOutputPortStandard<RentCarOutput> outputPort) : IUseCase<RentCarInput>
    {
        private const string MoreThanOneRentalMessage = "Not allowed more than 1 rental.";

        private readonly ICarRepository _carRepository = carRepository;
        private readonly IRentalRepository _rentalRepository = rentalRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IOutputPortStandard<RentCarOutput> _outputPort = outputPort;

        /// <inheritdoc/>
        public async Task Execute(RentCarInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            var existingRental = await _rentalRepository.GetActiveByUserIdAsync(input.UserId);
            if (existingRental != null)
            {
                throw new DomainException(MoreThanOneRentalMessage);
            }

            var car = await _carRepository.GetByIdAsync(input.CarId);
            if (car == null)
            {
                throw new NotFoundException("Car not found.");
            }

            var activeRentalForCar = await _rentalRepository.GetActiveByCarIdAsync(input.CarId);
            if (activeRentalForCar != null)
            {
                throw new DomainException("Car is already rented.");
            }

            var rental = new Rental
            {
                UserId = input.UserId,
                CarId = input.CarId,
                RentedAt = DateTime.UtcNow,
                ReturnedAt = null,
            };

            await _rentalRepository.AddAsync(rental);
            await _unitOfWork.Save();

            _outputPort.StandardHandle(new RentCarOutput { RentalId = rental.Id });
        }
    }
}
