using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.Presenters.ReturnCar;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.ReturnCar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Controllers
{
    /// <summary>
    /// Controller for returning a rented car.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ReturnController"/> class.
    /// </remarks>
    /// <param name="returnCarUseCase">The return car use case.</param>
    /// <param name="returnCarPresenter">The return car presenter.</param>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReturnController(
        IUseCase<ReturnCarInput> returnCarUseCase,
        ReturnCarPresenter returnCarPresenter) : ControllerBase
    {
        private readonly IUseCase<ReturnCarInput> _returnCarUseCase = returnCarUseCase;
        private readonly ReturnCarPresenter _returnCarPresenter = returnCarPresenter;

        /// <summary>
        /// Returns the specified car. Only the rent status for this car is updated; the vehicle is no longer in "rented" status.
        /// Any authenticated user can return any rented vehicle.
        /// </summary>
        /// <param name="carId">The car identifier to return.</param>
        /// <returns>200 OK with the rental id, or 404 if no active rental for this car.</returns>
        [HttpPost("{carId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ReturnCar(string carId)
        {
            var input = new ReturnCarInput { CarId = carId };

            await _returnCarUseCase.Execute(input);

            return _returnCarPresenter.ActionResult;
        }
    }
}
