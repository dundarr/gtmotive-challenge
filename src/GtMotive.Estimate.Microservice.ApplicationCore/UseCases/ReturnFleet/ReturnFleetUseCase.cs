using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.ReturnFleet
{
    /// <summary>
    /// Use case for returning a rented fleet vehicle. Marks the rental as returned so the fleet vehicle is no longer in "rented" status.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ReturnFleetUseCase"/> class.
    /// </remarks>
    /// <param name="rentalRepository">The rental repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="outputPort">The output port.</param>
    public class ReturnFleetUseCase(
        IRentalRepository rentalRepository,
        IUnitOfWork unitOfWork,
        IOutputPortStandard<ReturnFleetOutput> outputPort) : IUseCase<ReturnFleetInput>
    {
        private readonly IRentalRepository _rentalRepository = rentalRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IOutputPortStandard<ReturnFleetOutput> _outputPort = outputPort;

        /// <inheritdoc/>
        public async Task Execute(ReturnFleetInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            var rental = await _rentalRepository.GetActiveByFleetIdAsync(input.FleetId);
            if (rental == null)
            {
                throw new NotFoundException("No active rental found for this fleet vehicle.");
            }

            rental.ReturnedAt = DateTime.UtcNow;
            await _rentalRepository.UpdateAsync(rental);
            await _unitOfWork.Save();

            _outputPort.StandardHandle(new ReturnFleetOutput { RentalId = rental.Id });
        }
    }
}
