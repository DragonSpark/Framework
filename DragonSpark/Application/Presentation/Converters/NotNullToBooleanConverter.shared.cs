using System;
using System.Globalization;

namespace DragonSpark.Application.Presentation.Converters
{
	public class NotNullToBooleanConverter : BooleanConverter
	{
		public new static NotNullToBooleanConverter Instance
		{
			get { return InstanceField; }
		}	static readonly NotNullToBooleanConverter InstanceField = new NotNullToBooleanConverter();

		protected override bool Resolve(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value != null;
		}
	}
}