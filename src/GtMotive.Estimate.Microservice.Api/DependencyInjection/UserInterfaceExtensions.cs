using GtMotive.Estimate.Microservice.Api.Handlers.GetRentStatus;
using GtMotive.Estimate.Microservice.Api.Presenters.CreateFleet;
using GtMotive.Estimate.Microservice.Api.Presenters.GetAvailableFleets;
using GtMotive.Estimate.Microservice.Api.Presenters.GetFleets;
using GtMotive.Estimate.Microservice.Api.Presenters.GetRentStatus;
using GtMotive.Estimate.Microservice.Api.Presenters.RentFleet;
using GtMotive.Estimate.Microservice.Api.Presenters.ReturnFleet;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.CreateFleet;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetAvailableFleets;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetFleets;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetRentStatus;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.RentFleet;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.ReturnFleet;
using Microsoft.Extensions.DependencyInjection;

namespace GtMotive.Estimate.Microservice.Api.DependencyInjection
{
    public static class UserInterfaceExtensions
    {
        public static IServiceCollection AddPresenters(this IServiceCollection services)
        {
            services.AddScoped<CreateFleetPresenter>();
            services.AddScoped<IOutputPortStandard<CreateFleetOutput>>(sp => sp.GetRequiredService<CreateFleetPresenter>());
            services.AddScoped<GetAvailableFleetsPresenter>();
            services.AddScoped<IOutputPortStandard<GetAvailableFleetsOutput>>(sp => sp.GetRequiredService<GetAvailableFleetsPresenter>());
            services.AddScoped<GetFleetsPresenter>();
            services.AddScoped<IOutputPortStandard<GetFleetsOutput>>(sp => sp.GetRequiredService<GetFleetsPresenter>());
            services.AddScoped<GetRentStatusPresenter>();
            services.AddScoped<IOutputPortStandard<GetRentStatusOutput>>(sp => sp.GetRequiredService<GetRentStatusPresenter>());
            services.AddScoped<RentFleetPresenter>();
            services.AddScoped<IOutputPortStandard<RentFleetOutput>>(sp => sp.GetRequiredService<RentFleetPresenter>());
            services.AddScoped<ReturnFleetPresenter>();
            services.AddScoped<IOutputPortStandard<ReturnFleetOutput>>(sp => sp.GetRequiredService<ReturnFleetPresenter>());
            services.AddScoped<IGetRentStatusHandler, GetRentStatusHandler>();
            return services;
        }
    }
}
