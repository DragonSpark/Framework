using DragonSpark.Application;
using DragonSpark.Sources;
using JetBrains.Annotations;
using System;

namespace DragonSpark.Windows.FileSystem
{
	public sealed class TimestampNameFactory : SourceBase<string>
	{
		public static TimestampNameFactory Default { get; } = new TimestampNameFactory();
		TimestampNameFactory() : this( Clock.Default.Get ) {}

		readonly Func<DateTimeOffset> time;
		readonly string format;

		[UsedImplicitly]
		public TimestampNameFactory( Func<DateTimeOffset> time, string format = Defaults.ValidPathTimeFormat )
		{
			this.time = time;
			this.format = format;
		}

		public override string Get() => time().ToString( format );
	}
}