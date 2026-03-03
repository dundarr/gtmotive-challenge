using System;
using System.Linq;
using GtMotive.Estimate.Microservice.Api.Models;
using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetFleets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Presenters.GetFleets
{
    /// <summary>
    /// Presenter for the GetFleets use case output.
    /// </summary>
    public class GetFleetsPresenter : IWebApiPresenter, IOutputPortStandard<GetFleetsOutput>
    {
        /// <inheritdoc/>
        public IActionResult ActionResult { get; private set; }

        /// <inheritdoc/>
        public void StandardHandle(GetFleetsOutput response)
        {
            ArgumentNullException.ThrowIfNull(response);

            var fleets = response.Fleets.Select(f => new FleetResponse
            {
                Id = f.Id,
                PlateNumber = f.PlateNumber,
                Model = f.Model,
                DateOfManufacture = f.DateOfManufacture,
                KilometersRun = f.KilometersRun,
            }).ToList();

            ActionResult = new ObjectResult(fleets) { StatusCode = StatusCodes.Status200OK };
        }
    }
}
