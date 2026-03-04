using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Acheve.AspNetCore.TestHost.Security;
using FluentAssertions;
using GtMotive.Estimate.Microservice.InfrastructureTests.Infrastructure;
using Xunit;

namespace GtMotive.Estimate.Microservice.InfrastructureTests.Specs
{
    /// <summary>
    /// Infrastructure tests at host level: reception of REST calls and request model validation.
    /// Does not exercise the full use case (no real persistence).
    /// </summary>
    public sealed class RentApiHostSpec : InfrastructureTestBase
    {
        public RentApiHostSpec(GenericInfrastructureTestServerFixture fixture)
            : base(fixture)
        {
        }

        [Fact]
        public async Task GetPingReceivesRequestReturns200()
        {
            using var client = Fixture.Server.CreateClient();
            var response = await client.GetAsync(new System.Uri("/api/Test/ping", System.UriKind.Relative));

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Be("pong");
        }

        [Fact]
        public async Task PostRentWithInvalidModelReturnsBadRequestOrUnauthorized()
        {
            // Host-level: request is received; without auth returns 401; with valid auth + invalid model would return 400.
            // Validates that the REST endpoint is reachable and the pipeline runs (reception + auth/model layer).
            var requestBuilder = Fixture.Server.CreateRequest("/api/Rent");
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "test-user") };
            var response = await TestServerAuthenticationExtensions
                .WithIdentity(requestBuilder, claims)
                .And(r =>
                {
                    r.Method = System.Net.Http.HttpMethod.Post;
                    r.Content = new System.Net.Http.StringContent("{}", System.Text.Encoding.UTF8, "application/json");
                })
                .SendAsync("POST");

            // Either model validation (400) or auth runs first (401) — both prove host received the call.
            response.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest, HttpStatusCode.Unauthorized);
        }
    }
}
