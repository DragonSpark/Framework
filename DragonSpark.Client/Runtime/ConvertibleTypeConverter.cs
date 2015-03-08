using System;
using System.ComponentModel;

namespace DragonSpark.Runtime
{
	public class ConvertibleTypeConverter : TypeConverter
	{
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return typeof(IConvertible).IsAssignableFrom( destinationType );
		}

		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			var result = Convert.ChangeType( value, destinationType, null );
			return result;
		}
	}
}