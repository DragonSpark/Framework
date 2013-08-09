using System;
using System.Globalization;
using DragonSpark.Extensions;
using DragonSpark.Runtime;

namespace DragonSpark.Application.Presentation.Converters
{
	public class IsGreaterThanConverter : BooleanConverter
	{
		public new static IsGreaterThanConverter Instance
		{
			get { return InstanceField; }
		}	static readonly IsGreaterThanConverter InstanceField = new IsGreaterThanConverter();
		
		public int Target { get; set; }

		protected override bool Resolve(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var result = value.ConvertTo<int>() > Target;
			return result;
		}
	}
}