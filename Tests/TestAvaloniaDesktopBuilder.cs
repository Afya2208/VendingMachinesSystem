using Avalonia;
using Avalonia.Headless;
using DesktopApp;

[assembly: AvaloniaTestApplication(typeof(TestAvaloniaDesktopBuilder))]
public class TestAvaloniaDesktopBuilder
{
    public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<App>()
        .UseHeadless(new AvaloniaHeadlessPlatformOptions());
}


