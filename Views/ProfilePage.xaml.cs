using Codelingo.Frontend.Services;

namespace Codelingo.Frontend.Views;

public partial class ProfilePage : ContentPage
{
    private readonly ApiService _apiService;

    public ProfilePage(ApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadProfile();
    }

    private async Task LoadProfile()
    {
        var profile = await _apiService.GetProfile();

        if (profile == null)
        {
            MessageLabel.Text = "Failed to load profile.";
            return;
        }

        EmailLabel.Text = $"Email: {profile.Email}";
        PlanLabel.Text = profile.IsPremium ? "Plan: Premium" : "Plan: Free";
        LevelLabel.Text = $"Level: {profile.Level}";
        CurrentStreakLabel.Text = $"Current Streak: {profile.CurrentStreak}";
        LongestStreakLabel.Text = $"Longest Streak: {profile.LongestStreak}";
        CompletedLabel.Text = $"Quizzes Completed: {profile.TotalQuizzesCompleted}";
    }

    private async void OnRefreshClicked(object? sender, EventArgs e)
    {
        await LoadProfile();
    }
}