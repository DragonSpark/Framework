using System;
using System.Globalization;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Converters
{
	public class StringNotNullOrEmptyConverter : BooleanConverter
	{
		public new static StringNotNullOrEmptyConverter Instance
		{
			get { return InstanceField; }
		}	static readonly StringNotNullOrEmptyConverter InstanceField = new StringNotNullOrEmptyConverter();

		protected override bool Resolve(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var result = value.As<string>().Transform( x => !string.IsNullOrEmpty( x ) );
			return result;
		}
	}
}