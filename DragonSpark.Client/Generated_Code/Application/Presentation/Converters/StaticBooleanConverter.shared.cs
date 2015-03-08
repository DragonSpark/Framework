using System;
using System.Globalization;

namespace DragonSpark.Application.Presentation.Converters
{
	public class StaticBooleanConverter : BooleanConverter
	{
		public bool Value { get; set; }

		protected override bool Resolve(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Value;
		}
	}
}