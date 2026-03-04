using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.Handlers.GetRentStatus;
using GtMotive.Estimate.Microservice.Api.Models;
using GtMotive.Estimate.Microservice.Api.Presenters.CreateCar;
using GtMotive.Estimate.Microservice.Api.Presenters.GetAvailableCars;
using GtMotive.Estimate.Microservice.Api.Presenters.GetCars;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.CreateCar;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetAvailableCars;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetCars;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Controllers
{
    /// <summary>
    /// Controller for car operations.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CarController"/> class.
    /// </remarks>
    /// <param name="createCarUseCase">The create car use case.</param>
    /// <param name="createCarPresenter">The create car presenter.</param>
    /// <param name="getAvailableCarsUseCase">The get available cars use case.</param>
    /// <param name="getAvailableCarsPresenter">The get available cars presenter.</param>
    /// <param name="getCarsUseCase">The get cars use case.</param>
    /// <param name="getCarsPresenter">The get cars presenter.</param>
    /// <param name="getRentStatusHandler">The get rent status handler.</param>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
#pragma warning disable S6960 // Controller has multiple responsibilities - GET and POST for same resource is standard REST
    public class CarController(
        IUseCase<CreateCarInput> createCarUseCase,
        CreateCarPresenter createCarPresenter,
        IUseCase<GetAvailableCarsInput> getAvailableCarsUseCase,
        GetAvailableCarsPresenter getAvailableCarsPresenter,
        IUseCase<GetCarsInput> getCarsUseCase,
        GetCarsPresenter getCarsPresenter,
        IGetRentStatusHandler getRentStatusHandler) : ControllerBase
#pragma warning restore S6960
    {
        private readonly IUseCase<CreateCarInput> _createCarUseCase = createCarUseCase;
        private readonly CreateCarPresenter _createCarPresenter = createCarPresenter;
        private readonly IUseCase<GetAvailableCarsInput> _getAvailableCarsUseCase = getAvailableCarsUseCase;
        private readonly GetAvailableCarsPresenter _getAvailableCarsPresenter = getAvailableCarsPresenter;
        private readonly IUseCase<GetCarsInput> _getCarsUseCase = getCarsUseCase;
        private readonly GetCarsPresenter _getCarsPresenter = getCarsPresenter;
        private readonly IGetRentStatusHandler _getRentStatusHandler = getRentStatusHandler;

        /// <summary>
        /// Creates a new car.
        /// </summary>
        /// <param name="request">The car data.</param>
        /// <returns>201 Created with the new car id.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateCarRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var input = new CreateCarInput
            {
                PlateNumber = request.PlateNumber,
                Model = request.Model,
                DateOfManufacture = request.DateOfManufacture,
                KilometersRun = request.KilometersRun,
            };

            await _createCarUseCase.Execute(input);

            return _createCarPresenter.ActionResult;
        }

        /// <summary>
        /// Gets the list of available cars (not in rented status). Returns car data and rent status.
        /// </summary>
        /// <returns>200 OK with the list of available cars (car data and rent status).</returns>
        [HttpGet("available")]
        [ProducesResponseType(typeof(AvailableCarResponse[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAvailable()
        {
            await _getAvailableCarsUseCase.Execute(new GetAvailableCarsInput());
            return _getAvailableCarsPresenter.ActionResult;
        }

        /// <summary>
        /// Gets the list of cars stored in the database.
        /// </summary>
        /// <returns>200 OK with the list of cars.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(CarResponse[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            await _getCarsUseCase.Execute(new GetCarsInput());
            return _getCarsPresenter.ActionResult;
        }

        /// <summary>
        /// Gets the list of cars with car data and their rent status (Available or Rented).
        /// </summary>
        /// <returns>200 OK with the list of cars including rent status.</returns>
        [HttpGet("rentstatus")]
        [ProducesResponseType(typeof(AvailableCarResponse[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> RentStatus()
        {
            return await _getRentStatusHandler.GetRentStatusAsync();
        }
    }
}
