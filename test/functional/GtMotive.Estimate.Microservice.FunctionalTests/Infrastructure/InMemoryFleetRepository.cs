using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.FunctionalTests.Infrastructure
{
    internal sealed class InMemoryFleetRepository : IFleetRepository
    {
        private readonly List<Fleet> _fleets =
            [];

        public Task AddAsync(Fleet fleet)
        {
            fleet.Id ??= "fleet-" + (_fleets.Count + 1);
            _fleets.Add(fleet);
            return Task.CompletedTask;
        }

        public Task<Fleet> GetByIdAsync(string fleetId)
        {
            var fleet = _fleets.Find(f => f.Id == fleetId);
            return Task.FromResult(fleet);
        }

        public Task<IReadOnlyList<Fleet>> GetAllAsync()
        {
            return Task.FromResult<IReadOnlyList<Fleet>>(_fleets);
        }
    }
}
