using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.Handlers.GetRentStatus;
using GtMotive.Estimate.Microservice.Api.Models;
using GtMotive.Estimate.Microservice.Api.Presenters.CreateFleet;
using GtMotive.Estimate.Microservice.Api.Presenters.GetAvailableFleets;
using GtMotive.Estimate.Microservice.Api.Presenters.GetFleets;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.CreateFleet;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetAvailableFleets;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetFleets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Controllers
{
    /// <summary>
    /// Controller for fleet operations.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="FleetController"/> class.
    /// </remarks>
    /// <param name="createFleetUseCase">The create fleet use case.</param>
    /// <param name="createFleetPresenter">The create fleet presenter.</param>
    /// <param name="getAvailableFleetsUseCase">The get available fleets use case.</param>
    /// <param name="getAvailableFleetsPresenter">The get available fleets presenter.</param>
    /// <param name="getFleetsUseCase">The get fleets use case.</param>
    /// <param name="getFleetsPresenter">The get fleets presenter.</param>
    /// <param name="getRentStatusHandler">The get rent status handler.</param>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
#pragma warning disable S6960 // Controller has multiple responsibilities - GET and POST for same resource is standard REST
    public class FleetController(
        IUseCase<CreateFleetInput> createFleetUseCase,
        CreateFleetPresenter createFleetPresenter,
        IUseCase<GetAvailableFleetsInput> getAvailableFleetsUseCase,
        GetAvailableFleetsPresenter getAvailableFleetsPresenter,
        IUseCase<GetFleetsInput> getFleetsUseCase,
        GetFleetsPresenter getFleetsPresenter,
        IGetRentStatusHandler getRentStatusHandler) : ControllerBase
#pragma warning restore S6960
    {
        private readonly IUseCase<CreateFleetInput> _createFleetUseCase = createFleetUseCase;
        private readonly CreateFleetPresenter _createFleetPresenter = createFleetPresenter;
        private readonly IUseCase<GetAvailableFleetsInput> _getAvailableFleetsUseCase = getAvailableFleetsUseCase;
        private readonly GetAvailableFleetsPresenter _getAvailableFleetsPresenter = getAvailableFleetsPresenter;
        private readonly IUseCase<GetFleetsInput> _getFleetsUseCase = getFleetsUseCase;
        private readonly GetFleetsPresenter _getFleetsPresenter = getFleetsPresenter;
        private readonly IGetRentStatusHandler _getRentStatusHandler = getRentStatusHandler;

        /// <summary>
        /// Creates a new fleet vehicle.
        /// </summary>
        /// <param name="request">The fleet data.</param>
        /// <returns>201 Created with the new fleet id.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateFleetRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var input = new CreateFleetInput
            {
                PlateNumber = request.PlateNumber,
                Model = request.Model,
                DateOfManufacture = request.DateOfManufacture,
                KilometersRun = request.KilometersRun,
            };

            await _createFleetUseCase.Execute(input);

            return _createFleetPresenter.ActionResult;
        }

        /// <summary>
        /// Gets the list of available fleet vehicles (not in rented status). Returns fleet data and rent status.
        /// </summary>
        /// <returns>200 OK with the list of available fleet vehicles (fleet data and rent status).</returns>
        [HttpGet("available")]
        [ProducesResponseType(typeof(AvailableFleetResponse[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAvailable()
        {
            await _getAvailableFleetsUseCase.Execute(new GetAvailableFleetsInput());
            return _getAvailableFleetsPresenter.ActionResult;
        }

        /// <summary>
        /// Gets the list of fleet vehicles stored in the database.
        /// </summary>
        /// <returns>200 OK with the list of fleet vehicles.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(FleetResponse[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            await _getFleetsUseCase.Execute(new GetFleetsInput());
            return _getFleetsPresenter.ActionResult;
        }

        /// <summary>
        /// Gets the list of fleet vehicles with car data and their rent status (Available or Rented).
        /// </summary>
        /// <returns>200 OK with the list of fleet vehicles including rent status.</returns>
        [HttpGet("rentstatus")]
        [ProducesResponseType(typeof(AvailableFleetResponse[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> RentStatus()
        {
            return await _getRentStatusHandler.GetRentStatusAsync();
        }
    }
}
