using System;
using System.Linq;
using GtMotive.Estimate.Microservice.Api.Models;
using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetCars;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Presenters.GetCars
{
    /// <summary>
    /// Presenter for the GetCars use case output.
    /// </summary>
    public class GetCarsPresenter : IWebApiPresenter, IOutputPortStandard<GetCarsOutput>
    {
        /// <inheritdoc/>
        public IActionResult ActionResult { get; private set; }

        /// <inheritdoc/>
        public void StandardHandle(GetCarsOutput response)
        {
            ArgumentNullException.ThrowIfNull(response);

            var cars = response.Cars.Select(c => new CarResponse
            {
                Id = c.Id,
                PlateNumber = c.PlateNumber,
                Model = c.Model,
                DateOfManufacture = c.DateOfManufacture,
                KilometersRun = c.KilometersRun,
            }).ToList();

            ActionResult = new ObjectResult(cars) { StatusCode = StatusCodes.Status200OK };
        }
    }
}
