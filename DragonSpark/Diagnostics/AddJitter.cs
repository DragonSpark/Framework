using DragonSpark.Model.Selection.Alterations;
using System;

namespace DragonSpark.Diagnostics
{
	sealed class AddJitter : IAlteration<TimeSpan>
	{
		public static AddJitter Default { get; } = new AddJitter();

		AddJitter() : this(new Random()) {}

		readonly Random _random;

		public AddJitter(Random random) => _random = random;

		public TimeSpan Get(TimeSpan parameter) => parameter + TimeSpan.FromMilliseconds(_random.Next(0, 100));
	}
}