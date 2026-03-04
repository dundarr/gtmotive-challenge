using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Handlers.GetRentStatus
{
    /// <summary>
    /// Handler for the get rent status operation (car data and rent status).
    /// </summary>
    public interface IGetRentStatusHandler
    {
        /// <summary>
        /// Gets all cars with car data and their rent status.
        /// </summary>
        /// <returns>The action result with the list of cars and rent status.</returns>
        Task<IActionResult> GetRentStatusAsync();
    }
}
