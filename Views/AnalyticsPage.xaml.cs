using Codelingo.Frontend.Services;

namespace Codelingo.Frontend.Views;

public partial class AnalyticsPage : ContentPage
{
    private readonly ApiService _apiService;

    public AnalyticsPage(ApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadAnalytics();
    }

    private async Task LoadAnalytics()
    {
        MessageLabel.Text = "";

        var analytics = await _apiService.GetAnalyticsData();

        if (analytics == null)
        {
            AnalyticsLabel.Text = "Analytics locked. Upgrade to premium first.";
            return;
        }

        AnalyticsLabel.Text =
            $"Level: {analytics.Level}\n" +
            $"Current Streak: {analytics.CurrentStreak}\n" +
            $"Longest Streak: {analytics.LongestStreak}\n" +
            $"Quizzes Completed: {analytics.TotalQuizzesCompleted}\n" +
            $"Total Attempts: {analytics.TotalAttempts}\n" +
            $"Average Score: {analytics.AveragePercentage}%";
    }

    private async void OnRefreshClicked(object? sender, EventArgs e)
    {
        await LoadAnalytics();
    }

    private async void OnCopyResultsClicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(AnalyticsLabel.Text))
        {
            MessageLabel.Text = "No analytics to copy.";
            return;
        }

        await Clipboard.SetTextAsync(AnalyticsLabel.Text);

        MessageLabel.TextColor = Color.FromArgb("#BBF7D0");
        MessageLabel.Text = "Analytics copied to clipboard.";
    }
}