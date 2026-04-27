using Codelingo.Frontend.Models;
using Codelingo.Frontend.Services;

namespace Codelingo.Frontend.Views;

public partial class LearningMaterialPage : ContentPage
{
    private readonly ApiService _apiService;

    public LearningMaterialPage(ApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadMaterials();
    }

    private async Task LoadMaterials()
    {
        var materials = await _apiService.GetMyMaterials();
        MaterialsCollection.ItemsSource = materials;
    }

    private async void OnSaveClicked(object? sender, EventArgs e)
    {
        try
        {
            MessageLabel.Text = "";

            var request = new CreateLearningMaterialRequest
            {
                Title = TitleEntry.Text ?? "",
                Content = ContentEditor.Text ?? ""
            };

            var result = await _apiService.SaveLearningMaterial(request);

            if (result == null)
            {
                MessageLabel.TextColor = Colors.Red;
                MessageLabel.Text = "Failed to save material.";
                return;
            }

            Preferences.Set("current_material_id", result.Id);
            Preferences.Set("current_material_content", result.Content);

            TitleEntry.Text = "";
            ContentEditor.Text = "";

            MessageLabel.TextColor = Colors.Green;
            MessageLabel.Text = $"Saved material: {result.Title}";

            await LoadMaterials();
        }
        catch (Exception ex)
        {
            MessageLabel.TextColor = Colors.Red;
            MessageLabel.Text = ex.Message;
        }
    }

    private void OnUseClicked(object? sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is LearningMaterialResponse material)
        {
            Preferences.Set("current_material_id", material.Id);
            Preferences.Set("current_material_content", material.Content);

            MessageLabel.TextColor = Colors.Green;
            MessageLabel.Text = $"Selected material: {material.Title}";
        }
    }

    private async void OnDeleteClicked(object? sender, EventArgs e)
    {
        if (sender is not Button button || button.CommandParameter is not int id)
            return;

        bool deleted = await _apiService.DeleteLearningMaterial(id);

        if (!deleted)
        {
            MessageLabel.TextColor = Colors.Red;
            MessageLabel.Text = "Failed to delete material.";
            return;
        }

        MessageLabel.TextColor = Colors.Green;
        MessageLabel.Text = "Material deleted.";

        await LoadMaterials();
    }
}