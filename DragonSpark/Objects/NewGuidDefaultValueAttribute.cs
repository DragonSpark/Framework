using System;
using System.ComponentModel;
using System.Reflection;
using DragonSpark.Extensions;

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