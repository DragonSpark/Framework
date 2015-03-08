using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using DragonSpark.Extensions;
using DragonSpark.Objects.Synchronization;

namespace DragonSpark.Application.Presentation.Converters
{
	public class DisplayFieldConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			var result = value.FromMetadata<DisplayColumnAttribute, object>( x => value.EvaluateValue( x.DisplayColumn ), () => value );
			return result;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}
}