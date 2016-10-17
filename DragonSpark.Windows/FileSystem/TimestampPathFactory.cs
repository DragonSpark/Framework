using DragonSpark.Application;
using DragonSpark.Sources;
using System;

namespace DragonSpark.Windows.FileSystem
{
	public sealed class TimestampPathFactory : SourceBase<string>
	{
		public static TimestampPathFactory Default { get; } = new TimestampPathFactory();
		TimestampPathFactory() : this( CurrentTimeConfiguration.Default.Get ) {}

		readonly Func<ICurrentTime> time;

		public TimestampPathFactory( Func<ICurrentTime> time )
		{
			this.time = time;
		}

		public override string Get() => time().Now.ToString( Defaults.ValidPathTimeFormat );
	}
}