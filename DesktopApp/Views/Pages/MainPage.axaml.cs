using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using CommunityToolkit.Mvvm.Input;
using DesktopApp.ViewModels.Pages;

namespace DesktopApp.Views.Pages;

public partial class MainPage : UserControl
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void HideButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button)
        {
            var buttonTag = button.Tag as string ?? "";
            var sp = this.FindControl<StackPanel>(buttonTag);
            if (sp != null)
            {
                sp.IsVisible = !sp.IsVisible;
                button.Content = sp.IsVisible? "Скрыть" : "Показать";
            }
        }
    }

    private async void Page_OnLoaded(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainPageViewModel vm)
        {
            if (!vm.LoadDataCommand.IsRunning)
            {
                await vm.LoadDataCommand.ExecuteAsync(null);
                vm.CreateChartsCommand.Execute(null);
            }
        }
    }
}