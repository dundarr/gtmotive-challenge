using System;
using System.Security.Claims;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.Models;
using GtMotive.Estimate.Microservice.Api.Presenters.RentCar;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.RentCar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Controllers
{
    /// <summary>
    /// Controller for renting a car.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="RentController"/> class.
    /// </remarks>
    /// <param name="rentCarUseCase">The rent car use case.</param>
    /// <param name="rentCarPresenter">The rent car presenter.</param>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RentController(
        IUseCase<RentCarInput> rentCarUseCase,
        RentCarPresenter rentCarPresenter) : ControllerBase
    {
        private readonly IUseCase<RentCarInput> _rentCarUseCase = rentCarUseCase;
        private readonly RentCarPresenter _rentCarPresenter = rentCarPresenter;

        /// <summary>
        /// Rents a car. Only one car per person is allowed. User id is taken from the JWT token.
        /// If the user already has an active rental, returns 400 with message "Not allowed more than 1 rental".
        /// </summary>
        /// <param name="request">The rent request containing the car id.</param>
        /// <returns>201 Created with the rental id, or 400 if already renting, or 404 if car not found.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RentCar([FromBody] RentCarRequest request)
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

            var input = new RentCarInput
            {
                UserId = userId,
                CarId = request.CarId,
            };

            await _rentCarUseCase.Execute(input);

            return _rentCarPresenter.ActionResult;
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
