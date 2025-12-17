using System;
using Android.OS;
using DragonSpark.Model;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android;

sealed class IsProduct : FixedResultCondition<None>
{
    public static IsProduct Default { get; } = new();

    IsProduct() : this(Build.Product ?? string.Empty, StringComparison.OrdinalIgnoreCase) {}

    public IsProduct(string input, StringComparison comparison) : base(input.Contains("sdk_gphone", comparison)) {}
}