using System;
using System.Reflection;

namespace DragonSpark.ComponentModel
{
	[AttributeUsage( AttributeTargets.Property )]
	public sealed class CurrentTimeOffsetDefaultAttribute : DefaultPropertyValueAttribute
	{
		protected internal override object GetValue( object instance, PropertyInfo propertyInfo )
		{
			return DateTimeOffset.Now;
		}
	}
}