using System;
using System.ComponentModel;
using System.Reflection;

namespace DragonSpark.Objects
{
	[AttributeUsage( AttributeTargets.Property )]
	public sealed class CurrentTimeDefaultAttribute : DefaultPropertyValueAttribute
	{
		protected internal override object GetValue( object instance, PropertyInfo propertyInfo )
		{
			return DateTime.Now;
		}
	}
}