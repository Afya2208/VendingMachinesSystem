using Avalonia.Controls;
using Avalonia.Controls.Templates;
using DesktopApp.ViewModels;
using DesktopApp.ViewModels.Pages;
using DesktopApp.Views.Pages;

namespace DesktopApp;

public class ViewLocator : IDataTemplate
{
    public Control? Build(object? param)
    {
        if (param == null) return null;
        Control? control = param switch
        {
            MenuAndContentPageViewModel => new MenuAndContentPage(),
            AuthPageViewModel => new AuthPage(),
            InProcessPageViewModel => new InProcessPage(),
            MonitoringPageViewModel => new MonitoringPage(),
            MainPageViewModel => new MainPage(),
            CompaniesPageViewModel => new CompaniesPage(),
            CompanyFormPageViewModel => new CompanyFormPage(),
            VendingMachinesPageViewModel => new VendingMachinesPage(),
            VendingMachineFormPageViewModel => new VendingMachineFormPage(),
            _ => new TextBlock(){Text = "Not Found: " + param.GetType().Name}
        };
        return control;
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}