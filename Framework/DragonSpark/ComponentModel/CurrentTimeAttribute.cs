using DragonSpark.Runtime;
using System;
using System.Reflection;

namespace DragonSpark.ComponentModel
{
	[AttributeUsage( AttributeTargets.Property )]
	public sealed class CurrentTimeAttribute : DefaultAttribute
	{
		readonly ICurrentTime currentTime;

		public CurrentTimeAttribute() : this( CurrentTime.Instance )
		{}

		public CurrentTimeAttribute( ICurrentTime currentTime )
		{
			this.currentTime = currentTime;
		}

		protected internal override object GetValue( object instance, PropertyInfo propertyInfo )
		{
			var result = propertyInfo.PropertyType == typeof(DateTime) ? (object)currentTime.Now.DateTime : currentTime.Now;
			return result;
		}
	}
}