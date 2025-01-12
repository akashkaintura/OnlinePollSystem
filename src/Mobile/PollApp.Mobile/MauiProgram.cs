using Microsoft.Extensions.Logging;
using PollApp.Mobile.Services.Interfaces;
using PollApp.Mobile.Services.Implementations;
using PollApp.Mobile.ViewModels;
using PollApp.Mobile.Views;

namespace PollApp.Mobile;

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

        // Register Connectivity
        builder.Services.AddSingleton(Connectivity.Current);

        // HTTP Client Configuration
        builder.Services.AddHttpClient("PollApi", client =>
        {
            client.BaseAddress = new Uri(Constants.ApiBaseUrl);
        });

        // Service Registration
        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
        builder.Services.AddSingleton<IPollService, PollService>();
        builder.Services.AddSingleton<IAuthService, AuthService>();

        // ViewModel Registration
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<PollListViewModel>();
        builder.Services.AddTransient<PollDetailViewModel>();
        builder.Services.AddTransient<CreatePollViewModel>();

        // View Registration
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<PollListPage>();
        builder.Services.AddTransient<PollDetailPage>();
        builder.Services.AddTransient<CreatePollPage>();

        // Logging Configuration
#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}