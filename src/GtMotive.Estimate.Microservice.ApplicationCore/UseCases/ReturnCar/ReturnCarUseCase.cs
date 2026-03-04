using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.ReturnCar
{
    /// <summary>
    /// Use case for returning a rented car. Marks the rental as returned so the car is no longer in "rented" status.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ReturnCarUseCase"/> class.
    /// </remarks>
    /// <param name="rentalRepository">The rental repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="outputPort">The output port.</param>
    public class ReturnCarUseCase(
        IRentalRepository rentalRepository,
        IUnitOfWork unitOfWork,
        IOutputPortStandard<ReturnCarOutput> outputPort) : IUseCase<ReturnCarInput>
    {
        private readonly IRentalRepository _rentalRepository = rentalRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IOutputPortStandard<ReturnCarOutput> _outputPort = outputPort;

        /// <inheritdoc/>
        public async Task Execute(ReturnCarInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            var rental = await _rentalRepository.GetActiveByCarIdAsync(input.CarId);
            if (rental == null)
            {
                throw new NotFoundException("No active rental found for this car.");
            }

            rental.ReturnedAt = DateTime.UtcNow;
            await _rentalRepository.UpdateAsync(rental);
            await _unitOfWork.Save();

            _outputPort.StandardHandle(new ReturnCarOutput { RentalId = rental.Id });
        }
    }
}
