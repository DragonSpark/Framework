using System;
using System.Reflection;

namespace DragonSpark.Objects
{
	[AttributeUsage( AttributeTargets.Property )]
	public sealed class NewGuidDefaultValueAttribute : DefaultPropertyValueAttribute
	{
		protected internal override object GetValue( object instance, PropertyInfo propertyInfo )
		{
			var result = Guid.NewGuid();
			return result;
		}
	}
}