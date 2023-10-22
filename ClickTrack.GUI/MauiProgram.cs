using MauiIcons.Fluent;
using MauiIcons.FluentFilled;
using Microsoft.Extensions.Logging;

namespace ClickTrack.GUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        
        builder.UseMauiApp<App>().UseFluentMauiIcons();
        builder.UseMauiApp<App>().UseFluentFilledMauiIcons();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}