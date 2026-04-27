using Codelingo.Frontend.Models;
using Codelingo.Frontend.Services;

namespace Codelingo.Frontend.Views;

public partial class RegisterPage : ContentPage
{
    private readonly ApiService _apiService;

    public RegisterPage(ApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        MessageLabel.Text = "";

        var request = new RegisterRequest
        {   
            Name = NameEntry.Text ?? "",
            Email = EmailEntry.Text ?? "",
            Password = PasswordEntry.Text ?? ""
        };

        var result = await _apiService.Register(request);

        if (result.Contains("success", StringComparison.OrdinalIgnoreCase))
        {
            
            MessageLabel.TextColor = Colors.Green;
            MessageLabel.Text = "Account created. Go back and login.";
        }
        else
        {
            MessageLabel.TextColor = Colors.Red;
            MessageLabel.Text = result;
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}