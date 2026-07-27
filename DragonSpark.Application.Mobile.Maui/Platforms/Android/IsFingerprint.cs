using Android.OS;
using DragonSpark.Model;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android;

sealed class IsFingerprint : FixedResultCondition<None>
{
    public static IsFingerprint Default { get; } = new();

    IsFingerprint() : this(Build.Fingerprint ?? string.Empty, StringComparison.OrdinalIgnoreCase) {}

    public IsFingerprint(string input, StringComparison comparer)
        : base(input.Contains("generic", comparer) || input.Contains("unknown", comparer)) {}
}