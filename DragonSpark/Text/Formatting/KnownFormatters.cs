using System;
using DragonSpark.Model.Selection;

namespace DragonSpark.Text.Formatting
{
	sealed class KnownFormatters : Select<object, IFormattable>
	{
		public static KnownFormatters Default { get; } = new KnownFormatters();

		KnownFormatters() : base(FormatterRegistration.Default.Get) {}
	}
}