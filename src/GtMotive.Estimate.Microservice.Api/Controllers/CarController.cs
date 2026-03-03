using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.Models;
using GtMotive.Estimate.Microservice.Api.Presenters.CreateCar;
using GtMotive.Estimate.Microservice.Api.Presenters.GetCars;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.CreateCar;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetCars;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Controllers
{
    /// <summary>
    /// Controller for car operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
#pragma warning disable S6960 // Controller has multiple responsibilities - GET and POST for same resource is standard REST
    public class CarController : ControllerBase
#pragma warning restore S6960
    {
        private readonly IUseCase<CreateCarInput> _createCarUseCase;
        private readonly CreateCarPresenter _createCarPresenter;
        private readonly IUseCase<GetCarsInput> _getCarsUseCase;
        private readonly GetCarsPresenter _getCarsPresenter;

        /// <summary>
        /// Initializes a new instance of the <see cref="CarController"/> class.
        /// </summary>
        /// <param name="createCarUseCase">The create car use case.</param>
        /// <param name="createCarPresenter">The create car presenter.</param>
        /// <param name="getCarsUseCase">The get cars use case.</param>
        /// <param name="getCarsPresenter">The get cars presenter.</param>
        public CarController(
            IUseCase<CreateCarInput> createCarUseCase,
            CreateCarPresenter createCarPresenter,
            IUseCase<GetCarsInput> getCarsUseCase,
            GetCarsPresenter getCarsPresenter)
        {
            _createCarUseCase = createCarUseCase;
            _createCarPresenter = createCarPresenter;
            _getCarsUseCase = getCarsUseCase;
            _getCarsPresenter = getCarsPresenter;
        }

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
    }
}
