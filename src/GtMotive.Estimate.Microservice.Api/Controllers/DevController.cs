using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace GtMotive.Estimate.Microservice.Api.Controllers
{
    /// <summary>
    /// Development-only endpoints (e.g. seed data). Disabled when not running in Development.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class DevController(
        IHostEnvironment environment,
        ICarRepository carRepository,
        IRentalRepository rentalRepository,
        IUnitOfWork unitOfWork) : ControllerBase
    {
        private readonly IHostEnvironment _environment = environment;
        private readonly ICarRepository _carRepository = carRepository;
        private readonly IRentalRepository _rentalRepository = rentalRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        /// <summary>
        /// Populates the database with sample cars and one rental for local testing and review.
        /// Only available in Development. Idempotent: if data already exists, no changes are made.
        /// </summary>
        /// <returns>200 OK with a summary (created or already present).</returns>
        [HttpPost("seed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Seed()
        {
            if (!_environment.IsDevelopment())
            {
                return NotFound();
            }

            var existing = await _carRepository.GetAllAsync();
            if (existing.Count > 0)
            {
                return Ok(new
                {
                    message = "Data already present. No changes made.",
                    carsCount = existing.Count,
                });
            }

            var cars = new[]
            {
                new Car
                {
                    PlateNumber = "1234 ABC",
                    Model = "Sample Sedan",
                    DateOfManufacture = new DateOnly(2023, 6, 15),
                    KilometersRun = 15000,
                },
                new Car
                {
                    PlateNumber = "5678 XYZ",
                    Model = "Sample SUV",
                    DateOfManufacture = new DateOnly(2024, 1, 10),
                    KilometersRun = 5000,
                },
                new Car
                {
                    PlateNumber = "9012 DEF",
                    Model = "Sample Hatchback",
                    DateOfManufacture = new DateOnly(2022, 9, 1),
                    KilometersRun = 32000,
                },
            };

            foreach (var car in cars)
            {
                await _carRepository.AddAsync(car);
            }

            await _unitOfWork.Save();

            var firstCarId = cars[0].Id;
            var rental = new Rental
            {
                UserId = "seed-user-demo",
                CarId = firstCarId,
                RentedAt = DateTime.UtcNow.AddDays(-2),
                ReturnedAt = null,
            };

            await _rentalRepository.AddAsync(rental);
            await _unitOfWork.Save();

            return Ok(new
            {
                message = "Sample data created.",
                carsCreated = cars.Length,
                rentalsCreated = 1,
                carIds = new[] { cars[0].Id, cars[1].Id, cars[2].Id },
            });
        }
    }
}
