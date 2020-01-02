using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Text;
using DragonSpark.Text.Formatting;
using System;

namespace DragonSpark
{
	// ReSharper disable once MismatchedFileName

	public static partial class ExtensionMethods
	{
		/*public static IFormatter Register<T>(this ISelectFormatter<T> @this)
			=> FormatterRegistration.Default.Append(@this);*/

		public static IFormatter Append<T>(this IConditional<object, IFormattable> @this, ISelectFormatter<T> parameter)
		{
			var formatter = new Formatter<T>(Compose.Start.A.Selection.Of.Any.AndOf<T>()
			                                        .By.Cast.Or.Throw.Select(new Formatters<T>(parameter)));
			var result = new Formatter(@this.Unless(formatter));
			return result;
		}

		public static string OrNone<T>(this T @this) => @this?.ToString() ?? None.Default;

		public static string OrNone<T>(this T? @this) where T : struct => @this?.ToString() ?? None.Default;
	}
}