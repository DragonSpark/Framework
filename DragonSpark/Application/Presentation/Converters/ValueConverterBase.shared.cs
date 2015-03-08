using System;
using System.Globalization;
using System.Windows.Data;
using DragonSpark.Extensions;
using DragonSpark.Runtime;

namespace DragonSpark.Application.Presentation.Converters
{
	public abstract class ValueConverterBase<TValue,TParameter> : IValueConverter
	{
		object IValueConverter.Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			return Convert( value, targetType, parameter, culture );
		}

		protected virtual object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			var result = PerformConversion( value.To<TValue>(), parameter.To<TParameter>() );
			return result;
		}

		protected abstract object PerformConversion( TValue value, TParameter parameter );

		object IValueConverter.ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			return ConvertBack( value, targetType, parameter, culture );
		}

		protected virtual object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			var source = value.ConvertTo<TValue>();
			var result = PerformConversionBack( source, parameter.To<TParameter>() );
			return result;
		}

		protected virtual TValue PerformConversionBack( TValue source, TParameter target )
		{
			var result = source;
			return result;
		}
	}

	/*public class OperationsConverter : ValueConverterBase<IMetadataContainer,object>
	{
		public static OperationsConverter Instance
		{
			get { return InstanceField; }
		}	static readonly OperationsConverter InstanceField = new OperationsConverter();
		
		protected override object PerformConversion( IMetadataContainer value, object parameter )
		{
			var result = value.GetOperations();
			return result;
		}
	}

	public class MetadataConverter : ValueConverterBase<IMetadataContainer,Type>
	{
		public static MetadataConverter Instance
		{
			get { return InstanceField; }
		}	static readonly MetadataConverter InstanceField = new MetadataConverter();

		static readonly MethodInfo GetMetadata = typeof(IMetadataContainer).GetMethod( "GetMetadata" );

		protected override object PerformConversion( IMetadataContainer value, Type parameter )
		{
			var result = GetMetadata.MakeGenericMethod( parameter ).Invoke( value, new object[0] );
			return result;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}*/
}