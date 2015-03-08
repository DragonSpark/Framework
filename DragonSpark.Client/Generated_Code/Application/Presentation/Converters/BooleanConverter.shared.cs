using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Converters
{
	[ContentProperty( "Translator" )]
	public class BooleanConverter : IValueConverter
	{
		public static BooleanConverter Instance
		{
			get { return InstanceField; }
		}	static readonly BooleanConverter InstanceField = new BooleanConverter();
		
		public IValueConverter Translator { get; set; }

		public virtual object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			bool inverse;
			var visible = Resolve( value, targetType, parameter, culture );
			visible = parameter != null && bool.TryParse( parameter.ToString(), out inverse ) && inverse ? !visible : visible;
			var result = Translator.Transform( item => item.Convert( visible, targetType, parameter, culture ), () => visible );
			return result;
		}

		protected virtual bool Resolve( object value, Type targetType, object parameter, CultureInfo culture )
		{
			var result = System.Convert.ToBoolean( value );
			return result;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}
}