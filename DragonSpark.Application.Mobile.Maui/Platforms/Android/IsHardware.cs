using System;
using Android.OS;
using DragonSpark.Model;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android;

sealed class IsHardware : FixedResultCondition<None>
{
    public static IsHardware Default { get; } = new();

    IsHardware() : this(Build.Hardware ?? string.Empty, StringComparison.OrdinalIgnoreCase) {}

    public IsHardware(string input, StringComparison comparison)
        : base(input.Contains("goldfish", comparison) || input.Contains("ranchu", comparison)) {}
}