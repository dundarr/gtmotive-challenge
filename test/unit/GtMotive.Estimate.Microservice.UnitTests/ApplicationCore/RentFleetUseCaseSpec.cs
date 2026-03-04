using System;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.RentFleet;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using Moq;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.ApplicationCore
{
    /// <summary>
    /// Unit tests for RentFleetUseCase in isolation (mocked dependencies).
    /// </summary>
    public sealed class RentFleetUseCaseSpec
    {
        [Fact]
        public async Task ExecuteWhenUserAlreadyHasActiveRentalThrowsDomainException()
        {
            var userId = "user-1";
            var fleetId = "fleet-1";
            var existingRental = new Rental { Id = "r1", UserId = userId, FleetId = "other", ReturnedAt = null };
            var fleetRepo = new Mock<IFleetRepository>();
            var rentalRepo = new Mock<IRentalRepository>();
            rentalRepo.Setup(r => r.GetActiveByUserIdAsync(userId)).ReturnsAsync(existingRental);
            var unitOfWork = new Mock<IUnitOfWork>();
            var outputPort = new Mock<IOutputPortStandard<RentFleetOutput>>();

            var useCase = new RentFleetUseCase(
                fleetRepo.Object,
                rentalRepo.Object,
                unitOfWork.Object,
                outputPort.Object);

            var input = new RentFleetInput { UserId = userId, FleetId = fleetId };

            var act = () => useCase.Execute(input);

            await act.Should().ThrowAsync<DomainException>()
                .WithMessage("*Not allowed more than 1 rental*");
            outputPort.Verify(o => o.StandardHandle(It.IsAny<RentFleetOutput>()), Times.Never);
            unitOfWork.Verify(u => u.Save(), Times.Never);
        }

        [Fact]
        public async Task ExecuteWhenFleetNotFoundThrowsNotFoundException()
        {
            var userId = "user-1";
            var fleetId = "fleet-missing";
            var fleetRepo = new Mock<IFleetRepository>();
            fleetRepo.Setup(r => r.GetByIdAsync(fleetId)).ReturnsAsync((Fleet)null);
            var rentalRepo = new Mock<IRentalRepository>();
            rentalRepo.Setup(r => r.GetActiveByUserIdAsync(userId)).ReturnsAsync((Rental)null);
            var unitOfWork = new Mock<IUnitOfWork>();
            var outputPort = new Mock<IOutputPortStandard<RentFleetOutput>>();

            var useCase = new RentFleetUseCase(
                fleetRepo.Object,
                rentalRepo.Object,
                unitOfWork.Object,
                outputPort.Object);

            var input = new RentFleetInput { UserId = userId, FleetId = fleetId };

            var act = () => useCase.Execute(input);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("*Fleet not found*");
            outputPort.Verify(o => o.StandardHandle(It.IsAny<RentFleetOutput>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteWhenFleetAvailableCallsOutputPortAndSaves()
        {
            var userId = "user-1";
            var fleetId = "fleet-1";
            var fleet = new Fleet { Id = fleetId, PlateNumber = "P1", Model = "M1", DateOfManufacture = new DateOnly(2020, 1, 1), KilometersRun = 0 };
            var fleetRepo = new Mock<IFleetRepository>();
            fleetRepo.Setup(r => r.GetByIdAsync(fleetId)).ReturnsAsync(fleet);
            var rentalRepo = new Mock<IRentalRepository>();
            rentalRepo.Setup(r => r.GetActiveByUserIdAsync(userId)).ReturnsAsync((Rental)null);
            rentalRepo.Setup(r => r.GetActiveByFleetIdAsync(fleetId)).ReturnsAsync((Rental)null);
            rentalRepo.Setup(r => r.AddAsync(It.IsAny<Rental>())).Callback<Rental>(r => r.Id = "new-rental-id");
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(u => u.Save()).ReturnsAsync(1);
            var outputPort = new Mock<IOutputPortStandard<RentFleetOutput>>();

            var useCase = new RentFleetUseCase(
                fleetRepo.Object,
                rentalRepo.Object,
                unitOfWork.Object,
                outputPort.Object);

            var input = new RentFleetInput { UserId = userId, FleetId = fleetId };

            await useCase.Execute(input);

            outputPort.Verify(o => o.StandardHandle(It.Is<RentFleetOutput>(outp => outp.RentalId == "new-rental-id")), Times.Once);
            unitOfWork.Verify(u => u.Save(), Times.Once);
        }
    }
}
