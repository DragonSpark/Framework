using DragonSpark.Configuration;
using DragonSpark.Sources;
using System;

namespace DragonSpark.Application
{
	public sealed class Clock : ConfigurableSource<DateTimeOffset>, IClock
	{
		public static Clock Default { get; } = new Clock();
		Clock() : base( DefaultImplementation.Implementation.Get ) {}

		sealed class DefaultImplementation : SourceBase<DateTimeOffset>
		{
			public static DefaultImplementation Implementation { get; } = new DefaultImplementation();
			DefaultImplementation() {}

			public override DateTimeOffset Get() => DateTimeOffset.Now;
		}
	}
}