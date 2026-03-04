using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.FunctionalTests.Infrastructure
{
    internal sealed class InMemoryRentalRepository : IRentalRepository
    {
        private readonly List<Rental> _rentals =
            [];

        public Task<IReadOnlyList<string>> GetActiveRentedFleetIdsAsync()
        {
            var ids = _rentals.Where(r => r.ReturnedAt == null).Select(r => r.FleetId).Distinct().ToList();
            return Task.FromResult<IReadOnlyList<string>>(ids);
        }

        public Task AddAsync(Rental rental)
        {
            rental.Id ??= "rental-" + (_rentals.Count + 1);
            _rentals.Add(rental);
            return Task.CompletedTask;
        }

        public Task<Rental> GetActiveByUserIdAsync(string userId)
        {
            var rental = _rentals.Find(r => r.UserId == userId && r.ReturnedAt == null);
            return Task.FromResult(rental);
        }

        public Task<Rental> GetActiveByFleetIdAsync(string fleetId)
        {
            var rental = _rentals.Find(r => r.FleetId == fleetId && r.ReturnedAt == null);
            return Task.FromResult(rental);
        }

        public Task UpdateAsync(Rental rental)
        {
            var index = _rentals.FindIndex(r => r.Id == rental.Id);
            if (index >= 0)
            {
                _rentals[index] = rental;
            }

            return Task.CompletedTask;
        }
    }
}
