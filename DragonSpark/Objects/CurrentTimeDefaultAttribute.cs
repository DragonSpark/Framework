using System;
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

	[AttributeUsage( AttributeTargets.Property )]
	public sealed class CurrentTimeOffsetDefaultAttribute : DefaultPropertyValueAttribute
	{
		protected internal override object GetValue( object instance, PropertyInfo propertyInfo )
		{
			return DateTimeOffset.Now;
		}
	}
}