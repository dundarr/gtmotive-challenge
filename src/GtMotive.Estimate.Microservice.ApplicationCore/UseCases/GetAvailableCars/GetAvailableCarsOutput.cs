using System;
using System.Collections.Generic;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetAvailableCars
{
    /// <summary>
    /// Output for the GetAvailableCars use case.
    /// </summary>
    public class GetAvailableCarsOutput : IUseCaseOutput
    {
        /// <summary>
        /// Gets or sets the list of available cars (car data and rent status).
        /// </summary>
#pragma warning disable IDE0301 // Simplify collection initialization
        public IReadOnlyList<AvailableCarOutputItem> Cars { get; set; } = Array.Empty<AvailableCarOutputItem>();
#pragma warning restore IDE0301
    }
}
