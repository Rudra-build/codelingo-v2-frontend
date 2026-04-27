namespace Codelingo.Frontend.Models;

public class RegisterRequest
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginResponse
{
    public string Message { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Level { get; set; }
    public bool IsPremium { get; set; }
}

public class CreateLearningMaterialRequest
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}

public class LearningMaterialResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}

public class QuizResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public List<QuestionDto> Questions { get; set; } = new();
}

public class QuestionDto
{
    public int Id { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public List<OptionDto> Options { get; set; } = new();
}

public class OptionDto
{
    public int Id { get; set; }
    public string OptionText { get; set; } = string.Empty;
}

public class SubmitQuizRequest
{
    public int QuizId { get; set; }
    public List<AnswerDto> Answers { get; set; } = new();
}

public class AnswerDto
{
    public int QuestionId { get; set; }
    public int SelectedOptionId { get; set; }
}

public class GenerateQuizResponse
{
    public string Message { get; set; } = string.Empty;
    public int QuizId { get; set; }
    public int Level { get; set; }
}


public class CheckAnswerRequest
{
    public int QuizId { get; set; }
    public int QuestionId { get; set; }
    public int SelectedOptionId { get; set; }
}

public class CheckAnswerResponse
{
    public bool IsCorrect { get; set; }
    public string Message { get; set; } = string.Empty;
}


public class LeaderboardUser
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Level { get; set; }
    public int CurrentStreak { get; set; }
    public int TotalQuizzesCompleted { get; set; }
}


public class SubscriptionStatus
{
    public bool IsPremium { get; set; }
    public int Level { get; set; }
    public int CurrentStreak { get; set; }
}

public class AnalyticsResponse
{
    public int UserId { get; set; }
    public int Level { get; set; }
    public int CurrentStreak { get; set; }
    public int LongestStreak { get; set; }
    public int TotalQuizzesCompleted { get; set; }
    public int TotalAttempts { get; set; }
    public double AveragePercentage { get; set; }
}


public class CheckoutResponse
{
    public string CheckoutUrl { get; set; } = string.Empty;
    public string SessionId { get; set; } = string.Empty;
}

public class UserProfile
{
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; }
    public int CurrentStreak { get; set; }
    public int LongestStreak { get; set; }
    public int TotalQuizzesCompleted { get; set; }
    public bool IsPremium { get; set; }
}