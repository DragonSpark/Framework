using System;
using System.Reflection;
using DragonSpark.Extensions;

namespace DragonSpark.ComponentModel
{
	[AttributeUsage(AttributeTargets.Property)]
	public class DefaultAttribute : System.ComponentModel.DefaultValueAttribute
	{
		public DefaultAttribute() : base( null )
		{}

		public DefaultAttribute( char value ) : base( value )
		{}

		public DefaultAttribute( byte value ) : base( value )
		{}

		public DefaultAttribute( short value ) : base( value )
		{}

		public DefaultAttribute( int value ) : base( value )
		{}

		public DefaultAttribute( long value ) : base( value )
		{}

		public DefaultAttribute( float value ) : base( value )
		{}

		public DefaultAttribute( double value ) : base( value )
		{}

		public DefaultAttribute( bool value ) : base( value )
		{}

		public DefaultAttribute( string value ) : base( value )
		{}

		public DefaultAttribute( object value ) : base( value )
		{}

		protected internal virtual object GetValue( object instance, PropertyInfo propertyInfo )
		{
			var result = Value.ConvertTo( propertyInfo.PropertyType );
			return result;
		}
	}
}