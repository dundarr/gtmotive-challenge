using System;
using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.CreateFleet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Presenters.CreateFleet
{
    /// <summary>
    /// Presenter for the CreateFleet use case output.
    /// </summary>
    public class CreateFleetPresenter : IWebApiPresenter, IOutputPortStandard<CreateFleetOutput>
    {
        /// <inheritdoc/>
        public IActionResult ActionResult { get; private set; }

        /// <inheritdoc/>
        public void StandardHandle(CreateFleetOutput response)
        {
            ArgumentNullException.ThrowIfNull(response);

            ActionResult = new ObjectResult(new { id = response.Id }) { StatusCode = StatusCodes.Status201Created };
        }
    }
}
