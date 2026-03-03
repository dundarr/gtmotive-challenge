using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.Presenters.ReturnFleet;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.ReturnFleet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Controllers
{
    /// <summary>
    /// Controller for returning a rented fleet vehicle.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReturnController : ControllerBase
    {
        private readonly IUseCase<ReturnFleetInput> _returnFleetUseCase;
        private readonly ReturnFleetPresenter _returnFleetPresenter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReturnController"/> class.
        /// </summary>
        /// <param name="returnFleetUseCase">The return fleet use case.</param>
        /// <param name="returnFleetPresenter">The return fleet presenter.</param>
        public ReturnController(
            IUseCase<ReturnFleetInput> returnFleetUseCase,
            ReturnFleetPresenter returnFleetPresenter)
        {
            _returnFleetUseCase = returnFleetUseCase;
            _returnFleetPresenter = returnFleetPresenter;
        }

        /// <summary>
        /// Returns the specified fleet vehicle. Only the rent status for this fleet car is updated; the vehicle is no longer in "rented" status.
        /// Any authenticated user can return any rented vehicle.
        /// </summary>
        /// <param name="fleetId">The fleet vehicle identifier to return.</param>
        /// <returns>200 OK with the rental id, or 404 if no active rental for this fleet vehicle.</returns>
        [HttpPost("{fleetId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ReturnFleet(string fleetId)
        {
            var input = new ReturnFleetInput { FleetId = fleetId };

            await _returnFleetUseCase.Execute(input);

            return _returnFleetPresenter.ActionResult;
        }
    }
}
