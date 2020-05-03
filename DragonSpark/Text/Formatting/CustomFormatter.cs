using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Text.Formatting
{
	sealed class CustomFormatter : ICustomFormatter
	{
		readonly ISelect<object, IFormattable> _select;

		public CustomFormatter(ISelect<object, IFormattable> table) => _select = table;

		public string Format(string? format, object? arg, IFormatProvider? formatProvider)
			=> _select.Get(arg!).ToString(format, formatProvider);
	}
}