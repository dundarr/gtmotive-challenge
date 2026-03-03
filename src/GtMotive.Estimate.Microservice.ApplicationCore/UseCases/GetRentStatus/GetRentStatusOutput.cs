using System;
using System.Collections.Generic;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetRentStatus
{
    /// <summary>
    /// Output for the GetRentStatus use case.
    /// </summary>
    public class GetRentStatusOutput : IUseCaseOutput
    {
        /// <summary>
        /// Gets or sets the list of fleet vehicles with car data and rent status.
        /// </summary>
#pragma warning disable IDE0301 // Simplify collection initialization
        public IReadOnlyList<FleetRentStatusOutputItem> Fleets { get; set; } = Array.Empty<FleetRentStatusOutputItem>();
#pragma warning restore IDE0301
    }
}
