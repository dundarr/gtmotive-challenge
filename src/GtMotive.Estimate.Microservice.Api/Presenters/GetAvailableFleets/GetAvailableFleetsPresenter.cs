using System;
using System.Linq;
using GtMotive.Estimate.Microservice.Api.Models;
using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetAvailableFleets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Presenters.GetAvailableFleets
{
    /// <summary>
    /// Presenter for the GetAvailableFleets use case output.
    /// </summary>
    public class GetAvailableFleetsPresenter : IWebApiPresenter, IOutputPortStandard<GetAvailableFleetsOutput>
    {
        /// <inheritdoc/>
        public IActionResult ActionResult { get; private set; }

        /// <inheritdoc/>
        public void StandardHandle(GetAvailableFleetsOutput response)
        {
            ArgumentNullException.ThrowIfNull(response);

            var fleets = response.Fleets.Select(f => new AvailableFleetResponse
            {
                Id = f.Id,
                PlateNumber = f.PlateNumber,
                Model = f.Model,
                DateOfManufacture = f.DateOfManufacture,
                KilometersRun = f.KilometersRun,
                RentStatus = f.RentStatus,
            }).ToList();

            ActionResult = new ObjectResult(fleets) { StatusCode = StatusCodes.Status200OK };
        }
    }
}
