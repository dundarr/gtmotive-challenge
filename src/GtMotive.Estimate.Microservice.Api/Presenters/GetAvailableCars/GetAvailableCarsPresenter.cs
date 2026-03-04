using System;
using System.Linq;
using GtMotive.Estimate.Microservice.Api.Models;
using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetAvailableCars;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Presenters.GetAvailableCars
{
    /// <summary>
    /// Presenter for the GetAvailableCars use case output.
    /// </summary>
    public class GetAvailableCarsPresenter : IWebApiPresenter, IOutputPortStandard<GetAvailableCarsOutput>
    {
        /// <inheritdoc/>
        public IActionResult ActionResult { get; private set; }

        /// <inheritdoc/>
        public void StandardHandle(GetAvailableCarsOutput response)
        {
            ArgumentNullException.ThrowIfNull(response);

            var cars = response.Cars.Select(c => new AvailableCarResponse
            {
                Id = c.Id,
                PlateNumber = c.PlateNumber,
                Model = c.Model,
                DateOfManufacture = c.DateOfManufacture,
                KilometersRun = c.KilometersRun,
                RentStatus = c.RentStatus,
            }).ToList();

            ActionResult = new ObjectResult(cars) { StatusCode = StatusCodes.Status200OK };
        }
    }
}
