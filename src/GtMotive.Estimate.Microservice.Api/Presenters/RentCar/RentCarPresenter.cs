using System;
using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.RentCar;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Presenters.RentCar
{
    /// <summary>
    /// Presenter for the RentCar use case output.
    /// </summary>
    public class RentCarPresenter : IWebApiPresenter, IOutputPortStandard<RentCarOutput>
    {
        /// <inheritdoc/>
        public IActionResult ActionResult { get; private set; }

        /// <inheritdoc/>
        public void StandardHandle(RentCarOutput response)
        {
            ArgumentNullException.ThrowIfNull(response);

            ActionResult = new ObjectResult(new { rentalId = response.RentalId }) { StatusCode = StatusCodes.Status201Created };
        }
    }
}
