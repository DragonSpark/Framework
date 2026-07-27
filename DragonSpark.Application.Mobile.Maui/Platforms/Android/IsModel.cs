using Android.OS;
using DragonSpark.Model;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android;

sealed class IsModel : FixedResultCondition<None>
{
    public static IsModel Default { get; } = new();

    IsModel() : this(Build.Model ?? string.Empty, StringComparison.OrdinalIgnoreCase) {}

    public IsModel(string input, StringComparison comparer)
        : base(input.Contains("Emulator", comparer) || input.Contains("Simulator", comparer)) {}
}