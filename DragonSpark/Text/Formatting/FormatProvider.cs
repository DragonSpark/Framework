using DragonSpark.Model.Selection;
using DragonSpark.Runtime.Activation;
using System;
using System.Globalization;

namespace DragonSpark.Text.Formatting;

sealed class FormatProvider : ServiceProvider, IFormatProvider
{
	public FormatProvider(ISelect<object, IFormattable> table)
		: base(new CustomFormatter(table), CultureInfo.CurrentCulture) {}

	public object? GetFormat(Type? formatType) => GetService(formatType!);
}