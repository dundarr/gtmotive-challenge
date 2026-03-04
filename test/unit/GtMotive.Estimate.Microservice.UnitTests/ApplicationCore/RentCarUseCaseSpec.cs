using System;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.RentCar;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using Moq;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.ApplicationCore
{
    /// <summary>
    /// Unit tests for RentCarUseCase in isolation (mocked dependencies).
    /// </summary>
    public sealed class RentCarUseCaseSpec
    {
        [Fact]
        public async Task ExecuteWhenUserAlreadyHasActiveRentalThrowsDomainException()
        {
            var userId = "user-1";
            var carId = "car-1";
            var existingRental = new Rental { Id = "r1", UserId = userId, CarId = "other", ReturnedAt = null };
            var carRepo = new Mock<ICarRepository>();
            var rentalRepo = new Mock<IRentalRepository>();
            rentalRepo.Setup(r => r.GetActiveByUserIdAsync(userId)).ReturnsAsync(existingRental);
            var unitOfWork = new Mock<IUnitOfWork>();
            var outputPort = new Mock<IOutputPortStandard<RentCarOutput>>();

            var useCase = new RentCarUseCase(
                carRepo.Object,
                rentalRepo.Object,
                unitOfWork.Object,
                outputPort.Object);

            var input = new RentCarInput { UserId = userId, CarId = carId };

            var act = () => useCase.Execute(input);

            await act.Should().ThrowAsync<DomainException>()
                .WithMessage("*Not allowed more than 1 rental*");
            outputPort.Verify(o => o.StandardHandle(It.IsAny<RentCarOutput>()), Times.Never);
            unitOfWork.Verify(u => u.Save(), Times.Never);
        }

        [Fact]
        public async Task ExecuteWhenCarNotFoundThrowsNotFoundException()
        {
            var userId = "user-1";
            var carId = "car-missing";
            var carRepo = new Mock<ICarRepository>();
            carRepo.Setup(r => r.GetByIdAsync(carId)).ReturnsAsync((Car)null);
            var rentalRepo = new Mock<IRentalRepository>();
            rentalRepo.Setup(r => r.GetActiveByUserIdAsync(userId)).ReturnsAsync((Rental)null);
            var unitOfWork = new Mock<IUnitOfWork>();
            var outputPort = new Mock<IOutputPortStandard<RentCarOutput>>();

            var useCase = new RentCarUseCase(
                carRepo.Object,
                rentalRepo.Object,
                unitOfWork.Object,
                outputPort.Object);

            var input = new RentCarInput { UserId = userId, CarId = carId };

            var act = () => useCase.Execute(input);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("*Car not found*");
            outputPort.Verify(o => o.StandardHandle(It.IsAny<RentCarOutput>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteWhenCarAvailableCallsOutputPortAndSaves()
        {
            var userId = "user-1";
            var carId = "car-1";
            var car = new Car { Id = carId, PlateNumber = "P1", Model = "M1", DateOfManufacture = new DateOnly(2020, 1, 1), KilometersRun = 0 };
            var carRepo = new Mock<ICarRepository>();
            carRepo.Setup(r => r.GetByIdAsync(carId)).ReturnsAsync(car);
            var rentalRepo = new Mock<IRentalRepository>();
            rentalRepo.Setup(r => r.GetActiveByUserIdAsync(userId)).ReturnsAsync((Rental)null);
            rentalRepo.Setup(r => r.GetActiveByCarIdAsync(carId)).ReturnsAsync((Rental)null);
            rentalRepo.Setup(r => r.AddAsync(It.IsAny<Rental>())).Callback<Rental>(r => r.Id = "new-rental-id");
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(u => u.Save()).ReturnsAsync(1);
            var outputPort = new Mock<IOutputPortStandard<RentCarOutput>>();

            var useCase = new RentCarUseCase(
                carRepo.Object,
                rentalRepo.Object,
                unitOfWork.Object,
                outputPort.Object);

            var input = new RentCarInput { UserId = userId, CarId = carId };

            await useCase.Execute(input);

            outputPort.Verify(o => o.StandardHandle(It.Is<RentCarOutput>(outp => outp.RentalId == "new-rental-id")), Times.Once);
            unitOfWork.Verify(u => u.Save(), Times.Once);
        }
    }
}
