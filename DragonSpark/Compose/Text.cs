using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Text;
using DragonSpark.Text.Formatting;
using System;

namespace DragonSpark.Compose
{
	// ReSharper disable once MismatchedFileName

	public static partial class ExtensionMethods
	{
		public static IFormatter Append<T>(this IConditional<object, IFormattable> @this, ISelectFormatter<T> parameter)
		{
			var formatter = new Text.Formatting.Formatter<T>(Compose.Start.A.Selection.Of.Any.AndOf<T>()
			                                                        .By.Cast.Or.Throw.Select(new Formatters<T>(parameter))
			                                                        .Get());
			var result = new Formatter(@this.Then().Unless.Input.IsOf<T>().ThenUse(A.Selection(formatter)).Get());
			return result;
		}

		public static Selector<T, string> Intern<T>(this Selector<T, string> @this)
			=> @this.Select(Text.Intern.Default);

		public static string OrNone<T>(this T @this) => @this?.ToString() ?? None.Default;

		public static string OrNone<T>(this T? @this) where T : struct => @this?.ToString() ?? None.Default;
	}
}