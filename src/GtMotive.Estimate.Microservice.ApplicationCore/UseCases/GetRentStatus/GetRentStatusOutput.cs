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
        /// Gets or sets the list of cars with car data and rent status.
        /// </summary>
#pragma warning disable IDE0301 // Simplify collection initialization
        public IReadOnlyList<CarRentStatusOutputItem> Cars { get; set; } = Array.Empty<CarRentStatusOutputItem>();
#pragma warning restore IDE0301
    }
}
