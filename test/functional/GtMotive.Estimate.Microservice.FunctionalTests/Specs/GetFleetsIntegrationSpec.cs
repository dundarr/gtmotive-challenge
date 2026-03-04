using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Estimate.Microservice.Api.Presenters.GetFleets;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetFleets;
using GtMotive.Estimate.Microservice.FunctionalTests.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GtMotive.Estimate.Microservice.FunctionalTests.Specs
{
    /// <summary>
    /// Functional (integration) tests excluding the host: application + infrastructure, no HTTP.
    /// </summary>
    public sealed class GetFleetsIntegrationSpec : FunctionalTestBase
    {
        public GetFleetsIntegrationSpec(CompositionRootTestFixture fixture)
            : base(fixture)
        {
        }

        [Fact]
        public async Task GetFleetsUseCaseExecutesAndReturnsResultViaPresenter()
        {
            await Fixture.UsingScopeAsync(async scope =>
            {
                var useCase = scope.ServiceProvider.GetRequiredService<IUseCase<GetFleetsInput>>();
                var presenter = scope.ServiceProvider.GetRequiredService<GetFleetsPresenter>();

                await useCase.Execute(new GetFleetsInput());

                presenter.ActionResult.Should().NotBeNull();
                var objectResult = presenter.ActionResult as Microsoft.AspNetCore.Mvc.ObjectResult;
                objectResult.Should().NotBeNull();
                objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
                objectResult.Value.Should().NotBeNull();
            });
        }
    }
}
