using Codelingo.Frontend.Services;

namespace Codelingo.Frontend.Views;

public partial class HomePage : ContentPage
{
    private readonly ApiService _apiService;

    public HomePage(ApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    private async void OnAddMaterialClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(LearningMaterialPage));
    }

    private async void OnGenerateQuizClicked(object? sender, EventArgs e)
    {
        try
        {
            MessageLabel.TextColor = Colors.Black;
            MessageLabel.Text = "Generating quiz...";

            int materialId = Preferences.Get("current_material_id", 0);
            string content = Preferences.Get("current_material_content", "");

            if (materialId == 0 || string.IsNullOrWhiteSpace(content))
            {
                MessageLabel.TextColor = Colors.Red;
                MessageLabel.Text = "Please save learning material first.";
                return;
            }

            var result = await _apiService.GenerateQuiz(materialId, content);

            if (result == null)
            {
                MessageLabel.TextColor = Colors.Red;
                MessageLabel.Text = "Quiz generation failed.";
                return;
            }

            Preferences.Set("current_quiz_id", result.QuizId);

            MessageLabel.TextColor = Colors.Green;
            MessageLabel.Text = result.Message;

            await Shell.Current.GoToAsync(nameof(QuizPage));
        }
        catch (Exception ex)
        {
            MessageLabel.TextColor = Colors.Red;
            MessageLabel.Text = ex.Message;
        }


    }

    private async void OnLeaderboardClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(LeaderboardPage));
    }

    private async void OnSubscriptionClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SubscriptionPage));
    }

    private async void OnAnalyticsClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AnalyticsPage));
    }

    private async void OnLogoutClicked(object? sender, EventArgs e)
    {
        await _apiService.Logout();
        await Shell.Current.GoToAsync("//LoginPage");
    }

    private async void OnProfileClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ProfilePage));
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var profile = await _apiService.GetProfile();

        if (profile != null)
        {
            WelcomeLabel.Text = $"Welcome, {profile.Name}";
            StatsLabel.Text = $"Level {profile.Level} | Streak {profile.CurrentStreak} | {(profile.IsPremium ? "Premium" : "Free")}";
        }
    }
}