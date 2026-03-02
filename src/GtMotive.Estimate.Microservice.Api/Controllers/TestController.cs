using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class TestController : ControllerBase
    {
        /// <summary>
        /// Simple ping endpoint to verify the API is reachable.
        /// </summary>
        /// <returns>IActionResult.</returns>
        [HttpGet("ping")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Ping()
        {
            return Ok("pong");
        }

        /// <summary>
        /// Returns a simple status message for smoke testing.
        /// </summary>
        /// <returns>IActionResult.</returns>
        [HttpGet("status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Status()
        {
            return Ok(new { status = "ok", timestamp = DateTime.UtcNow });
        }
    }
}
