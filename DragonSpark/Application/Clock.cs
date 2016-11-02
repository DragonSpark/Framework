using DragonSpark.Sources.Scopes;
using System;

namespace DragonSpark.Application
{
	public sealed class Clock : ConfigurableSource<DateTimeOffset>, IClock
	{
		public static Clock Default { get; } = new Clock();
		Clock() : base( new Scope<DateTimeOffset>( () => DateTimeOffset.Now ) ) {}
	}
}