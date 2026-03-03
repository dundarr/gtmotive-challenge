using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.RentFleet
{
    /// <summary>
    /// Use case for renting a fleet vehicle. Only one active rental per user is allowed.
    /// </summary>
    public class RentFleetUseCase : IUseCase<RentFleetInput>
    {
        private const string MoreThanOneRentalMessage = "Not allowed more than 1 rental.";

        private readonly IFleetRepository _fleetRepository;
        private readonly IRentalRepository _rentalRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOutputPortStandard<RentFleetOutput> _outputPort;

        /// <summary>
        /// Initializes a new instance of the <see cref="RentFleetUseCase"/> class.
        /// </summary>
        /// <param name="fleetRepository">The fleet repository.</param>
        /// <param name="rentalRepository">The rental repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="outputPort">The output port.</param>
        public RentFleetUseCase(
            IFleetRepository fleetRepository,
            IRentalRepository rentalRepository,
            IUnitOfWork unitOfWork,
            IOutputPortStandard<RentFleetOutput> outputPort)
        {
            _fleetRepository = fleetRepository;
            _rentalRepository = rentalRepository;
            _unitOfWork = unitOfWork;
            _outputPort = outputPort;
        }

        /// <inheritdoc/>
        public async Task Execute(RentFleetInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            var existingRental = await _rentalRepository.GetActiveByUserIdAsync(input.UserId);
            if (existingRental != null)
            {
                throw new DomainException(MoreThanOneRentalMessage);
            }

            var fleet = await _fleetRepository.GetByIdAsync(input.FleetId);
            if (fleet == null)
            {
                throw new NotFoundException("Fleet not found.");
            }

            var activeRentalForFleet = await _rentalRepository.GetActiveByFleetIdAsync(input.FleetId);
            if (activeRentalForFleet != null)
            {
                throw new DomainException("Fleet vehicle is already rented.");
            }

            var rental = new Rental
            {
                UserId = input.UserId,
                FleetId = input.FleetId,
                RentedAt = DateTime.UtcNow,
                ReturnedAt = null,
            };

            await _rentalRepository.AddAsync(rental);
            await _unitOfWork.Save();

            _outputPort.StandardHandle(new RentFleetOutput { RentalId = rental.Id });
        }
    }
}
