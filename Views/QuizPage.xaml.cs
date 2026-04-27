// Views/QuizPage.xaml.cs
using Codelingo.Frontend.Models;
using Codelingo.Frontend.Services;

namespace Codelingo.Frontend.Views;

public partial class QuizPage : ContentPage
{
    private readonly ApiService _apiService;

    private QuizResponse? _quiz;
    private int _currentIndex = 0;
    private int _selectedOptionId = 0;
    private int _timeLeft = 15;
    private bool _answered = false;

    private readonly Dictionary<int, int> _selectedAnswers = new();

    public QuizPage(ApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        int quizId = Preferences.Get("current_quiz_id", 0);

        if (quizId == 0)
        {
            MessageLabel.TextColor = QuizPageStyles.ErrorText;
            MessageLabel.Text = "No quiz selected.";
            return;
        }

        _quiz = await _apiService.GetQuiz(quizId);

        if (_quiz == null || _quiz.Questions.Count == 0)
        {
            MessageLabel.TextColor = QuizPageStyles.ErrorText;
            MessageLabel.Text = "Failed to load quiz.";
            return;
        }

        _currentIndex = 0;
        _selectedAnswers.Clear();

        ShowCurrentQuestion();
    }

    private void ShowCurrentQuestion()
    {
        if (_quiz == null) return;

        _answered = false;
        _selectedOptionId = 0;
        _timeLeft = 15;

        var question = _quiz.Questions[_currentIndex];

        ProgressLabel.Text = $"Question {_currentIndex + 1} of {_quiz.Questions.Count}";
        ProgressBar.Progress = (double)_currentIndex / _quiz.Questions.Count;
        TimerLabel.Text = _timeLeft.ToString();
        TimerLabel.TextColor = QuizPageStyles.TimerRing;
        QuestionLabel.Text = question.QuestionText;

        OptionsLayout.Children.Clear();

        MessageLabel.TextColor = QuizPageStyles.SoftText;
        MessageLabel.Text = "Choose the best answer";

        foreach (var option in question.Options)
        {
            var button = new Button
            {
                Text = option.OptionText,
                CommandParameter = option.Id,
                BackgroundColor = QuizPageStyles.OptionDefault,
                TextColor = QuizPageStyles.TextPrimary,
                BorderColor = QuizPageStyles.OptionBorder,
                BorderWidth = 1,
                CornerRadius = 16,
                HeightRequest = 58,
                FontSize = 15,
                FontAttributes = FontAttributes.Bold,
                Padding = new Thickness(16, 0),
                HorizontalOptions = LayoutOptions.Fill
            };

            button.Clicked += OnOptionClicked;
            OptionsLayout.Children.Add(button);
        }

        StartTimer();
    }

    private void StartTimer()
    {
        Dispatcher.StartTimer(TimeSpan.FromSeconds(1), () =>
        {
            if (_answered)
                return false;

            _timeLeft--;
            TimerLabel.Text = _timeLeft.ToString();

            if (_timeLeft <= 5)
                TimerLabel.TextColor = QuizPageStyles.TimerWarning;

            if (_timeLeft <= 0)
            {
                _answered = true;

                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    MessageLabel.TextColor = QuizPageStyles.ErrorText;
                    MessageLabel.Text = "Time up!";
                    await DisplayAlert("Time Up", "You ran out of time.", "OK");
                    MoveNextOrSubmit();
                });

                return false;
            }

            return true;
        });
    }

    private async void OnOptionClicked(object? sender, EventArgs e)
    {
        if (_answered || _quiz == null)
            return;

        if (sender is not Button button || button.CommandParameter is not int optionId)
            return;

        _answered = true;
        _selectedOptionId = optionId;

        button.BackgroundColor = QuizPageStyles.OptionSelected;
        button.BorderColor = QuizPageStyles.OptionSelectedBorder;

        var question = _quiz.Questions[_currentIndex];
        _selectedAnswers[question.Id] = _selectedOptionId;

        MessageLabel.TextColor = QuizPageStyles.WarningText;
        MessageLabel.Text = "Checking answer...";

        var result = await _apiService.CheckAnswer(new CheckAnswerRequest
        {
            QuizId = _quiz.Id,
            QuestionId = question.Id,
            SelectedOptionId = _selectedOptionId
        });

        if (result == null)
        {
            MessageLabel.TextColor = QuizPageStyles.ErrorText;
            MessageLabel.Text = "Could not check answer.";
            await DisplayAlert("Error", "Could not check answer.", "OK");
            return;
        }

        if (result.Message.ToLower().Contains("correct"))
        {
            button.BackgroundColor = QuizPageStyles.CorrectGreen;
            button.BorderColor = QuizPageStyles.CorrectGreen;
            MessageLabel.TextColor = QuizPageStyles.SuccessText;
        }
        else
        {
            button.BackgroundColor = QuizPageStyles.WrongRed;
            button.BorderColor = QuizPageStyles.WrongRed;
            MessageLabel.TextColor = QuizPageStyles.ErrorText;
        }

        MessageLabel.Text = result.Message;

        await DisplayAlert("Answer", result.Message, "OK");

        MoveNextOrSubmit();
    }

    private async void MoveNextOrSubmit()
    {
        if (_quiz == null)
            return;

        _currentIndex++;

        if (_currentIndex < _quiz.Questions.Count)
        {
            ShowCurrentQuestion();
            return;
        }

        ProgressBar.Progress = 1;

        var request = new SubmitQuizRequest
        {
            QuizId = _quiz.Id,
            Answers = _selectedAnswers.Select(a => new AnswerDto
            {
                QuestionId = a.Key,
                SelectedOptionId = a.Value
            }).ToList()
        };

        string result = await _apiService.SubmitQuiz(request);

        Preferences.Set("latest_quiz_result", result);

        await Shell.Current.GoToAsync(nameof(ResultsPage));
    }
}