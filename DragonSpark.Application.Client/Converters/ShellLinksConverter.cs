using DragonSpark.Extensions;
using FirstFloor.ModernUI.Presentation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;
using IValueConverter = System.Windows.Data.IValueConverter;

namespace DragonSpark.Application.Client.Converters
{
	public class ShellLinksConverter : ConverterBase<IEnumerable<ToolbarItem>>
	{
		public static ShellLinksConverter Instance
		{
			get { return InstanceField; }
		}	static readonly ShellLinksConverter InstanceField = new ShellLinksConverter();

		protected override object PerformConversion( IEnumerable<ToolbarItem> value, object parameter )
		{
			var result = new LinkCollection( value.Select( item => new Link {	DisplayName = item.Text, Source = new Uri( string.Format( "http://{0}", Guid.NewGuid() ) ) } ) );
			return result;
		}
	}

	public class NegateConverter : ConverterBase<double>
	{
		public static NegateConverter Instance
		{
			get { return InstanceField; }
		}	static readonly NegateConverter InstanceField = new NegateConverter();
		protected override object PerformConversion( double value, object parameter )
		{
			var result = value * -1;
			return result;
		}
	}

	/*public class LinkGroupsConverter : ConverterBase<Page>
	{
		public static LinkGroupsConverter Instance
		{
			get { return InstanceField; }
		}	static readonly LinkGroupsConverter InstanceField = new LinkGroupsConverter();

		protected override object PerformConversion( Page value, object parameter )
		{

			var result = new LinkGroupCollection();
			var pages = value.AsTo<IPageContainer<Page>>()
			return result;
		}
	}*/

	public abstract class ConverterBase<TFrom> : ConverterBase<TFrom, object>
	{}

	public abstract class ConverterBase<TSource, TParameter> : IValueConverter
	{
		object IValueConverter.Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			return Convert( value, targetType, parameter, culture );
		}

		protected virtual object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			var result = PerformConversion( value.To<TSource>(), parameter.To<TParameter>() );
			return result;
		}

		protected abstract object PerformConversion( TSource value, TParameter parameter );

		object IValueConverter.ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			return ConvertBack( value, targetType, parameter, culture );
		}

		protected virtual object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			var source = value.ConvertTo<TSource>();
			var result = PerformConversionBack( source, parameter.To<TParameter>() );
			return result;
		}

		protected virtual TSource PerformConversionBack( TSource source, TParameter target )
		{
			var result = source;
			return result;
		}
	}
}
