using System;
using System.Linq;
using GtMotive.Estimate.Microservice.Api.Models;
using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetRentStatus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Presenters.GetRentStatus
{
    /// <summary>
    /// Presenter for the GetRentStatus use case output.
    /// </summary>
    public class GetRentStatusPresenter : IWebApiPresenter, IOutputPortStandard<GetRentStatusOutput>
    {
        /// <inheritdoc/>
        public IActionResult ActionResult { get; private set; }

        /// <inheritdoc/>
        public void StandardHandle(GetRentStatusOutput response)
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
