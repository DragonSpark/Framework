using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using DragonSpark.Extensions;

namespace DragonSpark.Client.Windows.Presentation
{
	public class DebugConverter : IValueConverter
	{
		public static DebugConverter Instance
		{
			get { return InstanceField; }
		}

		static readonly DebugConverter InstanceField = new DebugConverter();

		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			Debugger.Break();
			return value;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			Debugger.Break();
			return value;
		}
	}

	public class NullToBooleanConverter : BooleanConverter
	{
		public new static NullToBooleanConverter Instance
		{
			get { return InstanceField; }
		}	static readonly NullToBooleanConverter InstanceField = new NullToBooleanConverter();

		protected override bool Resolve( object value, Type targetType, object parameter, CultureInfo culture )
		{
			return value != null;
		}
	}

	[ContentProperty( "Translator" )]
	public class BooleanConverter : IValueConverter
	{
		public static BooleanConverter Instance
		{
			get { return InstanceField; }
		}	static readonly BooleanConverter InstanceField = new BooleanConverter();
		
		public IValueConverter Translator { get; set; }

		public bool Inverse { get; set; }

		public virtual object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			var resolved = Resolve( value, targetType, parameter, culture );
			var condition = Inverse || ConvertTo( parameter ) ? !resolved : resolved;
			var result = Translator.Transform( item => item.Convert( condition, targetType, parameter, culture ), () => condition );
			return result;
		}

		static bool ConvertTo( object parameter )
		{
			try
			{
				return parameter.ConvertTo<bool>();
			}
			catch ( FormatException )
			{
				return false;
			}
		}

		protected virtual bool Resolve( object value, Type targetType, object parameter, CultureInfo culture )
		{
			var result = ConvertTo( value );
			return result;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}

	public class BooleanToVisibilityTranslator : IValueConverter
	{
		public BooleanToVisibilityTranslator()
		{
			Off = Visibility.Collapsed;
		}

		public static BooleanToVisibilityTranslator Instance
		{
			get { return InstanceField; }
		}	static readonly BooleanToVisibilityTranslator InstanceField = new BooleanToVisibilityTranslator();

		public bool Inverse { get; set; }

		public Visibility Off { get; set; }

		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			var result = value.To<bool>() ? Visibility.Visible : Off;
			return result;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}

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
	
	public class StringFormatConverter : IValueConverter
	{
		public static StringFormatConverter Instance
		{
			get { return InstanceField; }
		}	static readonly StringFormatConverter InstanceField = new StringFormatConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var result = string.Format( parameter.Transform( x => x.ToString(), () => "{0}" ), value);
			return result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
