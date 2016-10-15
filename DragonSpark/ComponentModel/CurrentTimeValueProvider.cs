using DragonSpark.Application;
using System;
using System.Reflection;

namespace DragonSpark.ComponentModel
{
	public sealed class CurrentTimeValueProvider : DefaultValueProviderBase
	{
		public static CurrentTimeValueProvider Default { get; } = new CurrentTimeValueProvider();
		CurrentTimeValueProvider() : this( CurrentTimeConfiguration.Default.Get ) {}

		readonly Func<ICurrentTime> currentTime;

		public CurrentTimeValueProvider( Func<ICurrentTime> currentTime )
		{
			this.currentTime = currentTime;
		}

		public override object Get( PropertyInfo parameter )
		{
			var now = currentTime().Now;
			var result = parameter.PropertyType == typeof(DateTime) ? (object)now.DateTime : now;
			return result;
		}
	}
}