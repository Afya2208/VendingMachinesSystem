using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;

namespace DesktopApp.Views.Windows;

public partial class PushWindow : Window
{
    public string Text { get; set; }
    public char Symbol { get; set; }
    public bool ShowSecondCrossButton { get; set; }
    public Color Color  { get; set; }
    private TimeSpan _interval;
    private PushWindowType _pushWindowType;
    private DispatcherTimer _timer;

    public enum PushWindowType
    {
        Info,
        Warning,
        Error,
        Success
    }

    public PushWindow(PushWindowType type, string text)
    {
        _pushWindowType = type;
        Text = text;
        InitializeComponent();
        DefineIntervalAndColorAndSymbol();
        DataContext = this;
    }

    public void DefineIntervalAndColorAndSymbol()
    {
        switch (_pushWindowType)
        {
            case PushWindowType.Info:
                _interval = TimeSpan.FromSeconds(5);
                Color = Colors.LightSkyBlue;
                Symbol = 'ℹ';
                break;
            case PushWindowType.Warning:
                _interval = TimeSpan.FromSeconds(7);
                Color = Colors.LightSalmon;
                Symbol = '⚠';
                break;
            case PushWindowType.Error:
                _interval = TimeSpan.FromSeconds(10);
                Color = Colors.Red;
                Symbol = '❗';
                ShowSecondCrossButton = true;
                break;
            case PushWindowType.Success:
                _interval = TimeSpan.FromSeconds(5);
                Color = Colors.LightGreen;
                Symbol = '✅';
                ShowSecondCrossButton = true;
                break;
        }
        _timer = new DispatcherTimer();
        _timer.Interval = _interval;
        _timer.Tick += (_, _) => Close();
    }

    private void CloseButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}