using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Converters
{
	public class ComponentDescriptionConverter : IValueConverter
	{
		public static ComponentDescriptionConverter Instance
		{
			get { return InstanceField; }
		}	static readonly ComponentDescriptionConverter InstanceField = new ComponentDescriptionConverter();

		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			var transform = value.As<Enum>().Transform( x => x.GetType().GetField( x.ToString() ) ) ?? (ICustomAttributeProvider)value.GetType();
			var result = transform.FromMetadata<DescriptionAttribute, string>( x => x.Description, value.ToString );
			return result;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}
}