using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DesktopApp.ViewModels.Pages;

namespace DesktopApp.Views.Pages;

public partial class CompanyFormPage : UserControl
{
    public CompanyFormPage()
    {
        InitializeComponent();
    }
    
    private async void Page_OnLoaded(object? sender, RoutedEventArgs e)
    {
        if (DataContext is CompanyFormPageViewModel vm)
        {
            await vm.LoadDataCommand.ExecuteAsync(null);
        }
    }
}