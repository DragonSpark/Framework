using System;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using Activator = System.Activator;

namespace DragonSpark.Application.Presentation.Converters
{
	public class CoerceConverter : IValueConverter
	{
		// [TypeConverter( typeof(TypeNameConverter) )]
		public Type TargetType { get; set; }

		public object DefaultValue { get; set; }

		public bool AllowNull { get; set; }

		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			var result = value == null && AllowNull ? DefaultValue : CoerceValue( value );

			return result;
		}

		object CoerceValue( object value )
		{
			var coerceValue = value.ConvertTo( TargetType );
			var result = Equals( coerceValue, TargetType.GetDefaultValue() ) && value.Transform( x => TargetType.GetConstructors().FirstOrDefault( y => y.GetParameters().Transform( z => z.Count() == 1 && z.First().ParameterType == x.GetType() ) ) != null  ) ? Activator.CreateInstance( TargetType, value ) : coerceValue;
			return result;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}
}