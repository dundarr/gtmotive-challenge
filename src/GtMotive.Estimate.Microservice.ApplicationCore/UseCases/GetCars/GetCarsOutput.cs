using System;
using System.Collections.Generic;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetCars
{
    /// <summary>
    /// Output for the GetCars use case.
    /// </summary>
    public class GetCarsOutput : IUseCaseOutput
    {
        /// <summary>
        /// Gets or sets the list of cars.
        /// </summary>
#pragma warning disable IDE0301 // Simplify collection initialization
        public IReadOnlyList<CarOutputItem> Cars { get; set; } = Array.Empty<CarOutputItem>();
#pragma warning restore IDE0301
    }
}
