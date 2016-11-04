using DragonSpark.Sources;
using DragonSpark.Sources.Scopes;
using System;

namespace DragonSpark.Application
{
	public sealed class Clock : Scope<DateTimeOffset>, IClock
	{
		public static Clock Default { get; } = new Clock();
		Clock() : base( DefaultImplementation.Implementation.Get ) {}

		public sealed class DefaultImplementation : SourceBase<DateTimeOffset>
		{
			public static DefaultImplementation Implementation { get; } = new DefaultImplementation();
			DefaultImplementation() {}

			public override DateTimeOffset Get() => DateTimeOffset.Now;
		}
	}
}