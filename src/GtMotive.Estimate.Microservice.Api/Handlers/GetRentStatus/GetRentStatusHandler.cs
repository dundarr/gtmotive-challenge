using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.Presenters.GetRentStatus;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetRentStatus;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Handlers.GetRentStatus
{
    /// <summary>
    /// Handler for the get rent status operation.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="GetRentStatusHandler"/> class.
    /// </remarks>
    /// <param name="getRentStatusUseCase">The get rent status use case.</param>
    /// <param name="getRentStatusPresenter">The get rent status presenter.</param>
    public class GetRentStatusHandler(
        IUseCase<GetRentStatusInput> getRentStatusUseCase,
        GetRentStatusPresenter getRentStatusPresenter) : IGetRentStatusHandler
    {
        private readonly IUseCase<GetRentStatusInput> _getRentStatusUseCase = getRentStatusUseCase;
        private readonly GetRentStatusPresenter _getRentStatusPresenter = getRentStatusPresenter;

        /// <inheritdoc/>
        public async Task<IActionResult> GetRentStatusAsync()
        {
            await _getRentStatusUseCase.Execute(new GetRentStatusInput());
            return _getRentStatusPresenter.ActionResult;
        }
    }
}
