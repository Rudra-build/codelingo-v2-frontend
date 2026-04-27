namespace Codelingo.Frontend.Views;

public partial class ResultsPage : ContentPage
{
    public ResultsPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        ResultLabel.Text = Preferences.Get("latest_quiz_result", "No result found.");
    }

    private async void OnBackHomeClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(HomePage));
    }
}