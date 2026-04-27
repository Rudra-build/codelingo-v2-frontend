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

        EmailLabel.Text = profile.Email;
        PlanLabel.Text = profile.IsPremium ? "Premium" : "Free";
        LevelLabel.Text = profile.Level.ToString();
        CurrentStreakLabel.Text = profile.CurrentStreak.ToString();
        LongestStreakLabel.Text = profile.LongestStreak.ToString();
        CompletedLabel.Text = profile.TotalQuizzesCompleted.ToString();
    }

    private async void OnRefreshClicked(object? sender, EventArgs e)
    {
        await LoadProfile();
    }
}