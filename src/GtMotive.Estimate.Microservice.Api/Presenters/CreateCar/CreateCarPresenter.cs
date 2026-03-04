using System;
using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.CreateCar;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Presenters.CreateCar
{
    /// <summary>
    /// Presenter for the CreateCar use case output.
    /// </summary>
    public class CreateCarPresenter : IWebApiPresenter, IOutputPortStandard<CreateCarOutput>
    {
        /// <inheritdoc/>
        public IActionResult ActionResult { get; private set; }

        /// <inheritdoc/>
        public void StandardHandle(CreateCarOutput response)
        {
            ArgumentNullException.ThrowIfNull(response);

            ActionResult = new ObjectResult(new { id = response.Id }) { StatusCode = StatusCodes.Status201Created };
        }
    }
}
