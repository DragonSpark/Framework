using System;

namespace DragonSpark.Application.Compose.Store
{
	public readonly struct Slide
	{
		public Slide(TimeSpan @for) => For = @for;

		public TimeSpan For { get; }
	}
}