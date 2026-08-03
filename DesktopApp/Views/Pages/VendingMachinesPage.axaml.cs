using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DesktopApp.ViewModels.Pages;

namespace DesktopApp.Views.Pages;

public partial class VendingMachinesPage : UserControl
{
    public VendingMachinesPage()
    {
        InitializeComponent();
    }

    private async void Page_OnLoaded(object? sender, RoutedEventArgs e)
    {
        if (DataContext is VendingMachinesPageViewModel vm)
        {
            await vm.LoadDataCommand.ExecuteAsync(null);
        }
    }

    private async void CsvMenuItem_OnClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is VendingMachinesPageViewModel vm)
        { 
            var userSelectedPath= await AskUserPathWithDateToSave("csv","Список торговых автоматов");
            if (userSelectedPath != null)
            {
                await vm.CsvExportCommand.ExecuteAsync(userSelectedPath);
            }
        }
    }
    
    private async void HtmlMenuItem_OnClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is VendingMachinesPageViewModel vm)
        { 
            var userSelectedPath= await AskUserPathWithDateToSave("html","Список торговых автоматов");
            if (userSelectedPath != null)
            {
                await vm.HtmlExportCommand.ExecuteAsync(userSelectedPath);
            }
        }
    }
    private async void PdfMenuItem_OnClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is VendingMachinesPageViewModel vm)
        { 
            var userSelectedPath= await AskUserPathWithDateToSave("pdf","Список торговых автоматов");
            if (userSelectedPath != null)
            {
                await vm.PdfExportCommand.ExecuteAsync(userSelectedPath);
            }
        }
    }
}