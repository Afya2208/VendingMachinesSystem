using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace DesktopApp.Views.Pages;

public partial class AuthPage : UserControl
{
    public AuthPage()
    {
        InitializeComponent();
    }

    private void CloseButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var t = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
        t?.MainWindow?.Close();
    }
}