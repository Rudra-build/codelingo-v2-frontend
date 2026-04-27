using Codelingo.Frontend.Services;

namespace Codelingo.Frontend.Views;

public partial class LeaderboardPage : ContentPage
{
    private readonly ApiService _apiService;

    public LeaderboardPage(ApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var users = await _apiService.GetLeaderboard();

        if (users.Count == 0)
        {
            MessageLabel.Text = "No leaderboard data found.";
            return;
        }

        LeaderboardCollection.ItemsSource = users;
    }
}