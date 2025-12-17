using System;
using Android.OS;
using DragonSpark.Model;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android;

sealed class IsManufacturer : FixedResultCondition<None>
{
    public static IsManufacturer Default { get; } = new();

    IsManufacturer() : this(Build.Manufacturer ?? string.Empty, StringComparison.OrdinalIgnoreCase) {}

    public IsManufacturer(string input, StringComparison comparison) : base(input.Contains("Genymotion", comparison)) {}
}