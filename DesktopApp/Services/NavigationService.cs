using System;
using CommunityToolkit.Mvvm.ComponentModel;
using DesktopApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace DesktopApp.Services;

public partial class MainWindowNavigationService(IServiceProvider serviceProvider) : ObservableObject
{
    [ObservableProperty] private ViewModelBase? _currentViewModel;
    [ObservableProperty] private string? _title;
    
    public void NavigateTo<TViewModel>(string? title = null) where TViewModel : ViewModelBase
    {
        CurrentViewModel = serviceProvider.GetRequiredService<TViewModel>();
        if (title != null)
        {
            Title = title;
        }
    }
}