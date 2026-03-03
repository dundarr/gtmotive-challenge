using System;
using System.Security.Claims;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.Models;
using GtMotive.Estimate.Microservice.Api.Presenters.RentFleet;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.RentFleet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Controllers
{
    /// <summary>
    /// Controller for renting a fleet vehicle.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RentController : ControllerBase
    {
        private readonly IUseCase<RentFleetInput> _rentFleetUseCase;
        private readonly RentFleetPresenter _rentFleetPresenter;

        /// <summary>
        /// Initializes a new instance of the <see cref="RentController"/> class.
        /// </summary>
        /// <param name="rentFleetUseCase">The rent fleet use case.</param>
        /// <param name="rentFleetPresenter">The rent fleet presenter.</param>
        public RentController(
            IUseCase<RentFleetInput> rentFleetUseCase,
            RentFleetPresenter rentFleetPresenter)
        {
            _rentFleetUseCase = rentFleetUseCase;
            _rentFleetPresenter = rentFleetPresenter;
        }

        /// <summary>
        /// Rents a fleet vehicle. Only one fleet vehicle per person is allowed. User id is taken from the JWT token.
        /// If the user already has an active rental, returns 400 with message "Not allowed more than 1 rental".
        /// </summary>
        /// <param name="request">The rent request containing the fleet id.</param>
        /// <returns>201 Created with the rental id, or 400 if already renting, or 404 if fleet not found.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RentFleet([FromBody] RentFleetRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var userId = GetUserIdFromToken();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ProblemDetails
                {
                    Title = "Unauthorized",
                    Detail = "User identifier could not be read from the token.",
                    Status = StatusCodes.Status401Unauthorized,
                });
            }

            var input = new RentFleetInput
            {
                UserId = userId,
                FleetId = request.FleetId,
            };

            await _rentFleetUseCase.Execute(input);

            return _rentFleetPresenter.ActionResult;
        }

        /// <summary>
        /// Gets the user/person id from the JWT token (sub or NameIdentifier claim).
        /// </summary>
        private string GetUserIdFromToken()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value;
        }
    }
}
