using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Azure.Queues
{
	sealed class DefaultLife : Instance<TimeSpan>
	{
		public static DefaultLife Default { get; } = new DefaultLife();

		DefaultLife() : base(TimeSpan.FromDays(5)) {}
	}
}