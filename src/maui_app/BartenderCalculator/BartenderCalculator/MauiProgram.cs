using Bartender.SqlLite;
using BartenderCalculator.Alert;
using BartenderCalculator.Contracts;
using BartenderCalculator.ViewModels;
using BartenderCalculator.Views;
using Microsoft.Extensions.Logging;
using BartenderCalculator.Logging;

namespace BartenderCalculator;

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
                fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons");
            });
        //Database
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "products.db");
        var database = new Database(dbPath);
        builder.Services.AddSingleton<IDatabase>(database);
        
        // Register ViewModel
        builder.Services.AddTransient<ConfigurationViewModel>();
        builder.Services.AddTransient<ConfigurationView>();
        builder.Services.AddTransient<OrderViewModel>();
        builder.Services.AddTransient<OrderView>();
        
        // Alerts
        builder.Services.AddSingleton<IMessenger, MauiMessenger>();
        
        // Main Views
        builder.Services.AddSingleton<DesktopMainView>();
        builder.Services.AddSingleton<MobileMainView>();
        builder.Services.AddBartenderLogging();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}