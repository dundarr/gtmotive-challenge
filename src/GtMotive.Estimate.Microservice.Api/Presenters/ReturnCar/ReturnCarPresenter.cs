using System;
using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.ReturnCar;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Presenters.ReturnCar
{
    /// <summary>
    /// Presenter for the ReturnCar use case output.
    /// </summary>
    public class ReturnCarPresenter : IWebApiPresenter, IOutputPortStandard<ReturnCarOutput>
    {
        /// <inheritdoc/>
        public IActionResult ActionResult { get; private set; }

        /// <inheritdoc/>
        public void StandardHandle(ReturnCarOutput response)
        {
            ArgumentNullException.ThrowIfNull(response);

            ActionResult = new ObjectResult(new { rentalId = response.RentalId }) { StatusCode = StatusCodes.Status200OK };
        }
    }
}
