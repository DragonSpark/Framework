using System;
using System.Reflection;
using DragonSpark.Runtime;

namespace DragonSpark.Objects
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Justification = "Property is not meant to be exposed publicly."), AttributeUsage(AttributeTargets.Property)]
	public class DefaultPropertyValueAttribute : System.ComponentModel.DefaultValueAttribute
	{
		public DefaultPropertyValueAttribute() : base( null )
		{}

		public DefaultPropertyValueAttribute( char value ) : base( value )
		{}

		public DefaultPropertyValueAttribute( byte value ) : base( value )
		{}

		public DefaultPropertyValueAttribute( short value ) : base( value )
		{}

		public DefaultPropertyValueAttribute( int value ) : base( value )
		{}

		public DefaultPropertyValueAttribute( long value ) : base( value )
		{}

		public DefaultPropertyValueAttribute( float value ) : base( value )
		{}

		public DefaultPropertyValueAttribute( double value ) : base( value )
		{}

		public DefaultPropertyValueAttribute( bool value ) : base( value )
		{}

		public DefaultPropertyValueAttribute( string value ) : base( value )
		{}

		public DefaultPropertyValueAttribute( object value ) : base( value )
		{}

		public Type TypeConverterType { get; set; }

		protected internal virtual object GetValue( object instance, PropertyInfo propertyInfo )
		{
			var result = ConversionSupport.ConvertTo( Value, propertyInfo, TypeConverterType );
			return result;
		}
	}
}