using DragonSpark.Application;
using JetBrains.Annotations;
using System;
using System.Reflection;

namespace DragonSpark.ComponentModel
{
	public sealed class CurrentTimeValueProvider : DefaultValueProviderBase
	{
		public static CurrentTimeValueProvider Default { get; } = new CurrentTimeValueProvider();
		CurrentTimeValueProvider() : this( Clock.Default.Get ) {}

		readonly Func<DateTimeOffset> currentTime;

		[UsedImplicitly]
		public CurrentTimeValueProvider( Func<DateTimeOffset> currentTime )
		{
			this.currentTime = currentTime;
		}

		public override object Get( PropertyInfo parameter )
		{
			var now = currentTime();
			var result = parameter.PropertyType == typeof(DateTime) ? (object)now.DateTime : now;
			return result;
		}
	}
}