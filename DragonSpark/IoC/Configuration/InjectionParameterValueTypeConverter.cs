using System.ComponentModel;
using System.Globalization;

namespace DragonSpark.IoC.Configuration
{
	public class InjectionParameterValueTypeConverter : TypeConverter
	{
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			var result = new InstanceValue { Instance = value };
			return result;
		}
	}
}