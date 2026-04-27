using Codelingo.Frontend.Services;
using Codelingo.Frontend.Views;
using Microsoft.Extensions.Logging;

namespace Codelingo.Frontend;

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

        builder.Services.AddSingleton<ApiService>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterPage>();
		builder.Services.AddTransient<LearningMaterialPage>();
		builder.Services.AddTransient<HomePage>();
		builder.Services.AddTransient<QuizPage>();
		builder.Services.AddTransient<ResultsPage>();
		builder.Services.AddTransient<LeaderboardPage>();
		builder.Services.AddTransient<SubscriptionPage>();
		builder.Services.AddTransient<AnalyticsPage>();
		builder.Services.AddTransient<ProfilePage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}