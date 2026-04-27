using Codelingo.Frontend.Views;

namespace Codelingo.Frontend;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
        Routing.RegisterRoute(nameof(LearningMaterialPage), typeof(LearningMaterialPage));
		Routing.RegisterRoute(nameof(QuizPage), typeof(QuizPage));
		Routing.RegisterRoute(nameof(ResultsPage), typeof(ResultsPage));
		Routing.RegisterRoute(nameof(LeaderboardPage), typeof(LeaderboardPage));
		Routing.RegisterRoute(nameof(SubscriptionPage), typeof(SubscriptionPage));
		Routing.RegisterRoute(nameof(AnalyticsPage), typeof(AnalyticsPage));
		Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
    }
}