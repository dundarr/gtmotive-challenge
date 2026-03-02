using GtMotive.Estimate.Microservice.Api.Presenters.CreateCar;
using GtMotive.Estimate.Microservice.Api.Presenters.GetCars;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.CreateCar;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetCars;
using Microsoft.Extensions.DependencyInjection;

namespace GtMotive.Estimate.Microservice.Api.DependencyInjection
{
    public static class UserInterfaceExtensions
    {
        public static IServiceCollection AddPresenters(this IServiceCollection services)
        {
            services.AddScoped<CreateCarPresenter>();
            services.AddScoped<IOutputPortStandard<CreateCarOutput>>(sp => sp.GetRequiredService<CreateCarPresenter>());
            services.AddScoped<GetCarsPresenter>();
            services.AddScoped<IOutputPortStandard<GetCarsOutput>>(sp => sp.GetRequiredService<GetCarsPresenter>());
            return services;
        }
    }
}
