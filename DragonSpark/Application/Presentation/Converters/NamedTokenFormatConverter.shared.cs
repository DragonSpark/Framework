using System;
using System.Globalization;
using System.Windows.Data;
using DragonSpark.Extensions;
using DragonSpark.Runtime;

namespace DragonSpark.Application.Presentation.Converters
{
	public class NamedTokenFormatConverter : IValueConverter
	{
		public static NamedTokenFormatConverter Instance
		{
			get { return InstanceField; }
		}	static readonly NamedTokenFormatConverter InstanceField = new NamedTokenFormatConverter();

		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			var result = value.Transform( x => string.Format( NamedTokenFormatter.Instance, parameter.Transform( y => y.As<string>() ?? y.ToString(), () => "{0}" ), x ) );
			return result;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}
}