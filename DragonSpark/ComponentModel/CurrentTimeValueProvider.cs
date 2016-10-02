using DragonSpark.Application;
using System;

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

		public override object Get( DefaultValueParameter parameter )
		{
			var now = currentTime().Now;
			var result = parameter.Metadata.PropertyType == typeof(DateTime) ? (object)now.DateTime : now;
			return result;
		}
	}
}