using GtMotive.Estimate.Microservice.Api.Handlers.GetRentStatus;
using GtMotive.Estimate.Microservice.Api.Presenters.CreateCar;
using GtMotive.Estimate.Microservice.Api.Presenters.GetAvailableCars;
using GtMotive.Estimate.Microservice.Api.Presenters.GetCars;
using GtMotive.Estimate.Microservice.Api.Presenters.GetRentStatus;
using GtMotive.Estimate.Microservice.Api.Presenters.RentCar;
using GtMotive.Estimate.Microservice.Api.Presenters.ReturnCar;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.CreateCar;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetAvailableCars;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetCars;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetRentStatus;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.RentCar;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.ReturnCar;
using Microsoft.Extensions.DependencyInjection;

namespace GtMotive.Estimate.Microservice.Api.DependencyInjection
{
    public static class UserInterfaceExtensions
    {
        public static IServiceCollection AddPresenters(this IServiceCollection services)
        {
            services.AddScoped<CreateCarPresenter>();
            services.AddScoped<IOutputPortStandard<CreateCarOutput>>(sp => sp.GetRequiredService<CreateCarPresenter>());
            services.AddScoped<GetAvailableCarsPresenter>();
            services.AddScoped<IOutputPortStandard<GetAvailableCarsOutput>>(sp => sp.GetRequiredService<GetAvailableCarsPresenter>());
            services.AddScoped<GetCarsPresenter>();
            services.AddScoped<IOutputPortStandard<GetCarsOutput>>(sp => sp.GetRequiredService<GetCarsPresenter>());
            services.AddScoped<GetRentStatusPresenter>();
            services.AddScoped<IOutputPortStandard<GetRentStatusOutput>>(sp => sp.GetRequiredService<GetRentStatusPresenter>());
            services.AddScoped<RentCarPresenter>();
            services.AddScoped<IOutputPortStandard<RentCarOutput>>(sp => sp.GetRequiredService<RentCarPresenter>());
            services.AddScoped<ReturnCarPresenter>();
            services.AddScoped<IOutputPortStandard<ReturnCarOutput>>(sp => sp.GetRequiredService<ReturnCarPresenter>());
            services.AddScoped<IGetRentStatusHandler, GetRentStatusHandler>();
            return services;
        }
    }
}
