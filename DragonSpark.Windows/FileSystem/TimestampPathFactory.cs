using DragonSpark.Application;
using DragonSpark.Sources;
using System;

namespace DragonSpark.Windows.FileSystem
{
	public sealed class TimestampPathFactory : SourceBase<string>
	{
		public static TimestampPathFactory Default { get; } = new TimestampPathFactory();
		TimestampPathFactory() : this( Time.Default.Get ) {}

		readonly Func<DateTimeOffset> time;

		public TimestampPathFactory( Func<DateTimeOffset> time )
		{
			this.time = time;
		}

		public override string Get() => time().ToString( Defaults.ValidPathTimeFormat );
	}
}