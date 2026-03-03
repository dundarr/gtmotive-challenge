using System;

[assembly: CLSCompliant(false)]

namespace GtMotive.Estimate.Microservice.Host.Configuration
{
    internal sealed class AppSettings
    {
        public string JwtAuthority { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use local token endpoint and symmetric key validation instead of the external identity authority.
        /// Set to true in Development when the outer identity URL is unavailable; keep false in other environments.
        /// </summary>
        public bool UseLocalAuth { get; set; }

        /// <summary>
        /// Gets or sets the full URL for the local token endpoint (e.g. https://localhost:14602/connect/token).
        /// Used by Swagger when UseLocalAuth is true.
        /// </summary>
        public string LocalAuthTokenUrl { get; set; }
    }
}
