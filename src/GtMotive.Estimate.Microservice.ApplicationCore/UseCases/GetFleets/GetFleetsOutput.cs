using System;
using System.Collections.Generic;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetFleets
{
    /// <summary>
    /// Output for the GetFleets use case.
    /// </summary>
    public class GetFleetsOutput : IUseCaseOutput
    {
        /// <summary>
        /// Gets or sets the list of fleet vehicles.
        /// </summary>
#pragma warning disable IDE0301 // Simplify collection initialization
        public IReadOnlyList<FleetOutputItem> Fleets { get; set; } = Array.Empty<FleetOutputItem>();
#pragma warning restore IDE0301
    }
}
