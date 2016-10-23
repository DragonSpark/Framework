using DragonSpark.Sources;
using System;

namespace DragonSpark.Application
{
	public sealed class Time : DelegatedSource<DateTimeOffset>
	{
		public static Time Default { get; } = new Time();

		Time() : this( new Scope<ICurrentTime>() ) {}

		Time( IScope<ICurrentTime> configuration ) : base( configuration.GetCurrentDelegate() )
		{
			Configuration = configuration;
		}

		public IScope<ICurrentTime> Configuration { get; }

		/*public sealed class Configuration : Scope<ICurrentTime>
		{
			public static Configuration Default { get; } = new Configuration();
			Configuration() : base( () => CurrentTime.Default ) {}
		}*/
	}
}