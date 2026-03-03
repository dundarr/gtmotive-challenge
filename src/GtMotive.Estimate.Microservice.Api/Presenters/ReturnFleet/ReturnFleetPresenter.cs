using System;
using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.ReturnFleet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Presenters.ReturnFleet
{
    /// <summary>
    /// Presenter for the ReturnFleet use case output.
    /// </summary>
    public class ReturnFleetPresenter : IWebApiPresenter, IOutputPortStandard<ReturnFleetOutput>
    {
        /// <inheritdoc/>
        public IActionResult ActionResult { get; private set; }

        /// <inheritdoc/>
        public void StandardHandle(ReturnFleetOutput response)
        {
            ArgumentNullException.ThrowIfNull(response);

            ActionResult = new ObjectResult(new { rentalId = response.RentalId }) { StatusCode = StatusCodes.Status200OK };
        }
    }
}
