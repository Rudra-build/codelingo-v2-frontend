// Views/LeaderboardPageStyles.cs
using Microsoft.Maui.Graphics;

namespace Codelingo.Frontend.Views;

public static class LeaderboardPageStyles
{
    public static readonly Color Background = Color.FromArgb("#0A0F1E");
    public static readonly Color Card = Color.FromArgb("#111827");
    public static readonly Color Gold = Color.FromArgb("#FBBF24");
    public static readonly Color Silver = Color.FromArgb("#94A3B8");
    public static readonly Color Bronze = Color.FromArgb("#F97316");
    public static readonly Color TextPrimary = Colors.White;
    public static readonly Color SoftText = Color.FromArgb("#94A3B8");
    public static readonly Color AccentBlue = Color.FromArgb("#06B6D4");

    public static Color GetRankColor(int rank) => rank switch
    {
        1 => Gold,
        2 => Silver,
        3 => Bronze,
        _ => Color.FromArgb("#475569")
    };
}