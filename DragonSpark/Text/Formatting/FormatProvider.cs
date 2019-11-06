using System;
using System.Globalization;
using DragonSpark.Runtime.Activation;

namespace DragonSpark.Text.Formatting
{
	sealed class FormatProvider : ServiceProvider, IFormatProvider
	{
		public static FormatProvider Default { get; } = new FormatProvider();

		FormatProvider() : base(CustomFormatter.Default, CultureInfo.CurrentCulture) {}

		public object GetFormat(Type formatType) => GetService(formatType);
	}
}