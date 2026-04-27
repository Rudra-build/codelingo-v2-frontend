using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Codelingo.Frontend.Models;

namespace Codelingo.Frontend.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;

    private const string BaseUrl = "http://localhost:5058/api/";
    private const string TokenKey = "jwt_token";

    public ApiService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(BaseUrl)
        };
    }

    public async Task LoadSavedToken()
    {
        string? token = await SecureStorage.GetAsync(TokenKey);

        if (!string.IsNullOrWhiteSpace(token))
        {
            SetToken(token);
        }
    }

    public async Task SaveToken(string token)
    {
        await SecureStorage.SetAsync(TokenKey, token);
        SetToken(token);
    }

    public void SetToken(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task Logout()
    {
        SecureStorage.Remove(TokenKey);
        _httpClient.DefaultRequestHeaders.Authorization = null;
        await Task.CompletedTask;
    }

    public async Task<string> Register(RegisterRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/register", request);
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<LoginResponse?> Login(LoginRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/login", request);

        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

        if (result != null && !string.IsNullOrWhiteSpace(result.Token))
        {
            await SaveToken(result.Token);
        }

        return result;
    }

    public async Task<LearningMaterialResponse?> SaveLearningMaterial(CreateLearningMaterialRequest request)
    {
        await LoadSavedToken();

        var response = await _httpClient.PostAsJsonAsync("learningmaterial", request);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<LearningMaterialResponse>();
    }

    public async Task<List<LearningMaterialResponse>> GetMyMaterials()
    {
        await LoadSavedToken();

        var response = await _httpClient.GetAsync("learningmaterial/me");

        if (!response.IsSuccessStatusCode)
            return new List<LearningMaterialResponse>();

        return await response.Content.ReadFromJsonAsync<List<LearningMaterialResponse>>()
            ?? new List<LearningMaterialResponse>();
    }

    public async Task<bool> DeleteLearningMaterial(int id)
    {
        await LoadSavedToken();

        var response = await _httpClient.DeleteAsync($"learningmaterial/{id}");

        return response.IsSuccessStatusCode;
    }

    public async Task<GenerateQuizResponse?> GenerateQuiz(int materialId, string text)
    {
        await LoadSavedToken();

        var response = await _httpClient.PostAsJsonAsync($"quiz/generate?materialId={materialId}", text);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        return System.Text.Json.JsonSerializer.Deserialize<GenerateQuizResponse>(
            content,
            new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
    }

    public async Task<QuizResponse?> GetQuiz(int quizId)
    {
        await LoadSavedToken();

        var response = await _httpClient.GetAsync($"quiz/{quizId}");

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<QuizResponse>();
    }

    public async Task<string> SubmitQuiz(SubmitQuizRequest request)
    {
        await LoadSavedToken();

        var response = await _httpClient.PostAsJsonAsync("quiz/submit", request);
        return await response.Content.ReadAsStringAsync();
    }


    public async Task<CheckAnswerResponse?> CheckAnswer(CheckAnswerRequest request)
    {
        await LoadSavedToken();

        var response = await _httpClient.PostAsJsonAsync("quiz/check-answer", request);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<CheckAnswerResponse>();
    }
    public async Task<List<LeaderboardUser>> GetLeaderboard()
    {
        var response = await _httpClient.GetAsync("leaderboard");

        if (!response.IsSuccessStatusCode)
            return new List<LeaderboardUser>();

        return await response.Content.ReadFromJsonAsync<List<LeaderboardUser>>()
            ?? new List<LeaderboardUser>();
    }

    public async Task<string> GetAnalytics()
    {
        await LoadSavedToken();

        var response = await _httpClient.GetAsync("analytics/me");
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<CheckoutResponse?> CreateCheckoutSession()
    {
        await LoadSavedToken();

        var response = await _httpClient.PostAsync("subscription/create-checkout", null);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<CheckoutResponse>();
    }

    public async Task<string> ConfirmStripePayment()
    {
        await LoadSavedToken();

        var response = await _httpClient.PostAsync("subscription/confirm", null);
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<SubscriptionStatus?> GetSubscriptionStatus()
    {
        await LoadSavedToken();

        var response = await _httpClient.GetAsync("subscription/status");

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<SubscriptionStatus>();
    }

    public async Task<AnalyticsResponse?> GetAnalyticsData()
    {
        await LoadSavedToken();

        var response = await _httpClient.GetAsync("analytics/me");

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<AnalyticsResponse>();
    }

    public async Task<string> DowngradeSubscription()
    {
        await LoadSavedToken();

        var response = await _httpClient.PostAsync("subscription/downgrade", null);
        return await response.Content.ReadAsStringAsync();
    }


    public async Task<UserProfile?> GetProfile()
    {
        await LoadSavedToken();

        var response = await _httpClient.GetAsync("auth/me");

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<UserProfile>();
    }
}