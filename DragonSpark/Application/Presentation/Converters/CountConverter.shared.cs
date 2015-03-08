using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using DragonSpark.Extensions;
using DragonSpark.Runtime;

namespace DragonSpark.Application.Presentation.Converters
{
	public class CountConverter : BooleanConverter
	{
		public static CountConverter CountConverterInstance
		{
			get { return CountConverterInstanceField; }
		}	static readonly CountConverter CountConverterInstanceField = new CountConverter();
		
		public int Count { get; set; }

		protected override bool Resolve(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var count = value.AsTo<IEnumerable,int?>( item => item.Cast<object>().Count() ) ?? value.ConvertTo<int>();
			var result = count == Count;
			return result;
		}
	}
}