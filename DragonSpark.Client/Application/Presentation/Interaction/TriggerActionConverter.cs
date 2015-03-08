using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Interaction
{
	public class TriggerActionConverter : IValueConverter
	{
		public static TriggerActionConverter Instance
		{
			get { return InstanceField; }
		}	static readonly TriggerActionConverter InstanceField = new TriggerActionConverter();

		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			var result = value.As<DependencyObject>().Transform( x => System.Windows.Interactivity.Interaction.GetTriggers( x ).SelectMany( y => y.Actions ).FirstOrDefault( z => z.GetValue( FrameworkElement.NameProperty ).To<string>() == parameter.Transform( a => a.ToString() ) ) );
			return result;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}
}