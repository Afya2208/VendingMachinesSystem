using System;
using System.Net.Http;
using DesktopApp.Services;
using DesktopApp.ViewModels;
using DesktopApp.ViewModels.Pages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DesktopApp;

public static class ServiceCollectionExtensions
{
    public static void AddMainServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<IConfiguration>(config);
        services.AddSingleton<AuthService>();
        services.AddSingleton<MainWindowNavigationService>();
        services.AddSingleton<MenuNavigationService>();
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton(new HttpClient()
        {
            BaseAddress = new Uri(config["ApiUrl"])
        });
        services.AddTransient<AuthPageViewModel>();
        services.AddTransient<CompaniesPageViewModel>();
        services.AddTransient<MainPageViewModel>();
        services.AddTransient<InProcessPageViewModel>();
        services.AddTransient<MenuAndContentPageViewModel>();
        services.AddTransient<CompanyFormPageViewModel>();
        services.AddTransient<MonitoringPageViewModel>();
        services.AddTransient<VendingMachineFormPageViewModel>();
        services.AddTransient<VendingMachinesPageViewModel>();
    }
}