using System;
using System.Reflection;
using DragonSpark.Extensions;
using DragonSpark.Runtime;

namespace DragonSpark.ComponentModel
{
	public class FactoryDefaultValueAttribute : ActivationDefaultAttribute
	{
		public FactoryDefaultValueAttribute( Type type ) : base( type )
		{}

		protected internal override object GetValue( object instance, PropertyInfo propertyInfo )
		{
			var value = base.GetValue( instance, propertyInfo ).AsTo<IFactory,object>( x => x.Create( propertyInfo.PropertyType ) );
			return value;
		}
	}
}