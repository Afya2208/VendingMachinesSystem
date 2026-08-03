
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using DesktopApp.Services;
using DesktopApp.ViewModels;
using DesktopApp.ViewModels.Pages;
using DesktopApp.Views;
using DesktopApp.Views.Pages;
using Microsoft.Extensions.DependencyInjection;
using Models.Dto;
using NSubstitute;

namespace Tests;

public class DesktopTests
{
    private readonly ServiceProvider _serviceProvider;

    public DesktopTests()
    {
        var services = new ServiceCollection();
        services.AddSingleton<MainWindowNavigationService>();
        services.AddSingleton<MenuNavigationService>();
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<AuthPageViewModel>();
        services.AddTransient<MenuAndContentPageViewModel>();
        services.AddSingleton<AuthService>();
        services.AddSingleton(new HttpClient()
        {
            BaseAddress = new Uri("http://localhost:x/")
        });
        _serviceProvider = services.BuildServiceProvider();
    }
    
    
    [AvaloniaFact]
    public void AuthPageTestInput()
    {
        var window = new Window();
        
        var authPage = new AuthPage()
        {
            DataContext = _serviceProvider.GetService<AuthPageViewModel>()
        };
        window.Content = authPage;
        var emailTextBox = authPage.FindControl<TextBox>("Email");
        var passwordTextBox = authPage.FindControl<TextBox>("Password");

        emailTextBox.Focus();
        window.KeyTextInput("111");
        passwordTextBox.Focus();
        window.KeyTextInput("111");
        var authVM = authPage.DataContext as AuthPageViewModel;
        
        
        Assert.Equal(emailTextBox.Text, authVM.Email);
        Assert.Equal(passwordTextBox.Text, authVM.Password);
    }
    
    
    [AvaloniaFact]
    public void AuthPageAuthorizationTest()
    {
        var window = new Window();
        
        var authPage = new AuthPage()
        {
            DataContext = _serviceProvider.GetService<AuthPageViewModel>()
        };
        window.Content = authPage;
        var emailTextBox = authPage.FindControl<TextBox>("Email");
        var passwordTextBox = authPage.FindControl<TextBox>("Password");

        emailTextBox.Focus();
        window.KeyTextInput("123");
        passwordTextBox.Focus();
        window.KeyTextInput("123");
        
        var signInButton = authPage.FindControl<Button>("SignInButton");
        signInButton.Focus();
        // todo fix click on signInButton
        window.KeyPressQwerty(PhysicalKey.Space, RawInputModifiers.None);
        
        var authVM = authPage.DataContext as AuthPageViewModel;
        
        Assert.True(authVM.SignInCommand.IsRunning);
    }
}