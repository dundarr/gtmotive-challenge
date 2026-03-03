using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Handlers.GetRentStatus
{
    /// <summary>
    /// Handler for the get rent status operation (fleet data and rent status).
    /// </summary>
    public interface IGetRentStatusHandler
    {
        /// <summary>
        /// Gets all fleet vehicles with car data and their rent status.
        /// </summary>
        /// <returns>The action result with the list of fleet vehicles and rent status.</returns>
        Task<IActionResult> GetRentStatusAsync();
    }
}
