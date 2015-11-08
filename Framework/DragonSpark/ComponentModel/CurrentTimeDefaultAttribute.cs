using System;
using System.Reflection;

namespace DragonSpark.ComponentModel
{
	[AttributeUsage( AttributeTargets.Property )]
	public sealed class CurrentTimeAttribute : DefaultAttribute
	{
		protected internal override object GetValue( object instance, PropertyInfo propertyInfo )
		{
			var result = propertyInfo.PropertyType == typeof(DateTime) ? (object)DateTime.Now : DateTimeOffset.Now;
			return result;
		}
	}
}