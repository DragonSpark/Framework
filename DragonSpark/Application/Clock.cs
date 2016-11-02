using DragonSpark.Sources.Scopes;
using System;

namespace DragonSpark.Application
{
	public sealed class Clock : Scope<DateTimeOffset>, IClock
	{
		public static Clock Default { get; } = new Clock();
		Clock() : base( () => DateTimeOffset.Now ) {}
	}
}