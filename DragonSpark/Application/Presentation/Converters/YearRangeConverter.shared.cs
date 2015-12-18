using System;

namespace DragonSpark.Application.Presentation.Converters
{
	public class YearRangeConverter : ValueConverterBase<DateTime,object>
	{
		protected override object PerformConversion( DateTime value, object parameter )
		{
			var result = string.Concat( value.Year, DateTime.Today.Year <= value.Year ? string.Empty : string.Format( "-{0}", DateTime.Today.Year ) );
			return result;
		}
	}
}