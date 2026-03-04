using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.FunctionalTests.Infrastructure
{
    internal sealed class InMemoryCarRepository : ICarRepository
    {
        private readonly List<Car> _cars =
            [];

        public Task AddAsync(Car car)
        {
            car.Id ??= "car-" + (_cars.Count + 1);
            _cars.Add(car);
            return Task.CompletedTask;
        }

        public Task<Car> GetByIdAsync(string carId)
        {
            var car = _cars.Find(c => c.Id == carId);
            return Task.FromResult(car);
        }

        public Task<IReadOnlyList<Car>> GetAllAsync()
        {
            return Task.FromResult<IReadOnlyList<Car>>(_cars);
        }
    }
}
