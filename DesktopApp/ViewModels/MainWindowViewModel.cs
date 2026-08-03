using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using DesktopApp.Services;
using DesktopApp.ViewModels.Pages;
using DesktopApp.Views;
using DesktopApp.Views.Pages;
using Microsoft.Extensions.DependencyInjection;
using Models.Dto;
using Models.Entities;

namespace DesktopApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public MainWindowNavigationService NavigationService { get; }
    
    public MainWindowViewModel(MainWindowNavigationService navigationService)
    {
        NavigationService = navigationService;
        NavigateToAuthPage();
    }
    
    public void NavigateToAuthPage()
    {
        NavigationService.NavigateTo<AuthPageViewModel>("Авторизация");
    }
}