using Codelingo.Frontend.Models;
using Codelingo.Frontend.Services;

namespace Codelingo.Frontend.Views;

public partial class LoginPage : ContentPage
{
    private readonly ApiService _apiService;

    public LoginPage(ApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    private async void OnLoginClicked(object? sender, EventArgs e)
    {
        try
        {
            MessageLabel.TextColor = Colors.Black;
            MessageLabel.Text = "Logging in...";

            var request = new LoginRequest
            {
                Email = EmailEntry.Text ?? "",
                Password = PasswordEntry.Text ?? ""
            };

            var result = await _apiService.Login(request);

            if (result == null)
            {
                MessageLabel.TextColor = Colors.Red;
                MessageLabel.Text = "Login failed. Check email/password/backend.";
                return;
            }

            MessageLabel.TextColor = Colors.Green;
            MessageLabel.Text = $"Welcome {result.Email}";

            await Shell.Current.GoToAsync(nameof(HomePage));
        }
        catch (Exception ex)
        {
            MessageLabel.TextColor = Colors.Red;
            MessageLabel.Text = ex.Message;
        }
    }

    private async void OnRegisterClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(RegisterPage));
    }
}