using System;
using System.Reflection;
using DragonSpark.Activation;
using DragonSpark.Extensions;

namespace DragonSpark.ComponentModel
{
	public sealed class FactoryAttribute : ActivateAttribute
	{
		public FactoryAttribute( Type activatedType ) : base( activatedType )
		{}

		protected internal override object GetValue( object instance, PropertyInfo propertyInfo )
		{
			var value = base.GetValue( instance, propertyInfo ).AsTo<IFactory, object>( x => x.Create( propertyInfo.PropertyType ) );
			return value;
		}
	}
}