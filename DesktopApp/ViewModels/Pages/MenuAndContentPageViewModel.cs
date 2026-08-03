using System;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopApp.Services;
using DesktopApp.Views.Pages;
using Microsoft.Extensions.DependencyInjection;
using Models.Dto;

namespace DesktopApp.ViewModels.Pages;

public partial class MenuAndContentPageViewModel : ViewModelBase
{
    private MainWindowNavigationService _mainWindowNavigationService;
    private AuthService _authService;
    
    [ObservableProperty] private bool _isPaneOpened = true;
    [ObservableProperty] private MenuNavigationService _navigationService;
    [ObservableProperty] private string _fullName;
    [ObservableProperty] private Bitmap? _image;
    [ObservableProperty] private bool? _authorized;
    [ObservableProperty] private string _role;
    
    public MenuAndContentPageViewModel(AuthService authService, MenuNavigationService navigationService,
        MainWindowNavigationService mainWindowNavigationService)
    {
        _authService = authService;
        _mainWindowNavigationService = mainWindowNavigationService;
        _navigationService = navigationService;
        Authorize();
        _navigationService.NavigateTo<MainPageViewModel>("Главная");
    }
    private void Authorize()
    {
        Authorized = true;
        var fullname = _authService.LastName;
        if (_authService.FirstName != null)
        {
            fullname += $" {_authService.FirstName.First()}.";
        }
        if (_authService.MiddleName != null)
        {
            fullname += $" {_authService.MiddleName.First()}.";
        }
        FullName = fullname;
        Role = _authService.Role.Name;
        if (_authService.Image ==  null)
        {
            var uriDefaultImage = new Uri("avares://DesktopApp/Assets/default.jpg");
            using var stream = AssetLoader.Open(uriDefaultImage);
            Image = new Bitmap(stream);
        }
        else
        {
            using var stream = new MemoryStream(_authService.Image);
            _authService.Image = null;
            Image = new Bitmap(stream);
        }
    }
    
    public void MenuButtonClick()
    {
        IsPaneOpened = !IsPaneOpened;
    }
    
    public void MenuOptionClick(string option)
    {
        switch (option)
        {
            case "Главная":
                _navigationService.NavigateTo<MainPageViewModel>(option);
                break;
            case "Монитор ТА":
                _navigationService.NavigateTo<MonitoringPageViewModel>(option);
                break;
            case "Торговые автоматы":
                _navigationService.NavigateTo<VendingMachinesPageViewModel>(option);
                break;
            case "Компании":
                _navigationService.NavigateTo<CompaniesPageViewModel>(option);
                break;
            default:
                _navigationService.NavigateTo<InProcessPageViewModel>(option);
                break;
        }
    }

    [RelayCommand]
    public void SignOut()
    {
        _mainWindowNavigationService.NavigateTo<AuthPageViewModel>();
        _authService.ClearUserData();
    }
}