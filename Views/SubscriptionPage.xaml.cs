using Codelingo.Frontend.Services;

namespace Codelingo.Frontend.Views;

public partial class SubscriptionPage : ContentPage
{
    private readonly ApiService _apiService;

    public SubscriptionPage(ApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadStatus();
    }

    private async Task LoadStatus()
    {
        MessageLabel.Text = "";

        var status = await _apiService.GetSubscriptionStatus();

        if (status == null)
        {
            MessageLabel.TextColor = Colors.Red;
            MessageLabel.Text = "Failed to load subscription status.";
            return;
        }

        StatusLabel.Text = status.IsPremium ? "Plan: Premium" : "Plan: Free";
        LevelLabel.Text = $"Level: {status.Level}";
        StreakLabel.Text = $"Current Streak: {status.CurrentStreak}";
    }

    private async void OnRefreshClicked(object? sender, EventArgs e)
    {
        await LoadStatus();
    }

    private async void OnStripeClicked(object? sender, EventArgs e)
    {
        var checkout = await _apiService.CreateCheckoutSession();

        if (checkout == null || string.IsNullOrWhiteSpace(checkout.CheckoutUrl))
        {
            MessageLabel.TextColor = Colors.Red;
            MessageLabel.Text = "Failed to create Stripe checkout.";
            return;
        }

        await Launcher.OpenAsync(checkout.CheckoutUrl);
        MessageLabel.TextColor = Colors.Green;
        MessageLabel.Text = "Complete payment in Stripe, then return and press Confirm Payment.";
    }

    private async void OnConfirmClicked(object? sender, EventArgs e)
    {
        string result = await _apiService.ConfirmStripePayment();

        MessageLabel.TextColor = result.Contains("upgraded") ? Colors.Green : Colors.Red;
        MessageLabel.Text = result;

        await LoadStatus();
    }

    private async void OnDowngradeClicked(object? sender, EventArgs e)
    {
        string result = await _apiService.DowngradeSubscription();

        MessageLabel.TextColor = Colors.Green;
        MessageLabel.Text = result;

        await LoadStatus();
    }
}