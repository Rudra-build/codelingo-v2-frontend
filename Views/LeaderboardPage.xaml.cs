// Views/LeaderboardPage.xaml.cs
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

        MessageLabel.Text = "";

        var users = await _apiService.GetLeaderboard();

        if (users.Count == 0)
        {
            MessageLabel.Text = "No leaderboard data found.";
            return;
        }

        LeaderboardCollection.ItemsSource = users.Select((user, index) =>
        {
            int rank = index + 1;

            return new
            {
                RankDisplay = rank == 1 ? "🥇" : rank == 2 ? "🥈" : rank == 3 ? "🥉" : rank.ToString(),
                RankColor = LeaderboardPageStyles.GetRankColor(rank),
                user.Name,
                user.Level,
                user.CurrentStreak,
                user.TotalQuizzesCompleted
            };
        }).ToList();
    }
}