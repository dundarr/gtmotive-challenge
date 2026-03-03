using System;
using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.RentFleet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Presenters.RentFleet
{
    /// <summary>
    /// Presenter for the RentFleet use case output.
    /// </summary>
    public class RentFleetPresenter : IWebApiPresenter, IOutputPortStandard<RentFleetOutput>
    {
        /// <inheritdoc/>
        public IActionResult ActionResult { get; private set; }

        /// <inheritdoc/>
        public void StandardHandle(RentFleetOutput response)
        {
            ArgumentNullException.ThrowIfNull(response);

            ActionResult = new ObjectResult(new { rentalId = response.RentalId }) { StatusCode = StatusCodes.Status201Created };
        }
    }
}
