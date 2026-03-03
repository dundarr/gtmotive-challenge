using System;
using System.Collections.Generic;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetAvailableFleets
{
    /// <summary>
    /// Output for the GetAvailableFleets use case.
    /// </summary>
    public class GetAvailableFleetsOutput : IUseCaseOutput
    {
        /// <summary>
        /// Gets or sets the list of available fleet vehicles (fleet data and rent status).
        /// </summary>
#pragma warning disable IDE0301 // Simplify collection initialization
        public IReadOnlyList<AvailableFleetOutputItem> Fleets { get; set; } = Array.Empty<AvailableFleetOutputItem>();
#pragma warning restore IDE0301
    }
}
