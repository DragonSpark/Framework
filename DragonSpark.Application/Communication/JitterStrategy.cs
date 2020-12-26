using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Application.Communication
{
	sealed class JitterStrategy : Select<int, TimeSpan>
	{
		public static JitterStrategy Default { get; } = new JitterStrategy();

		JitterStrategy() : base(RetryStrategy.Default.Then().Select(AddJitter.Default)) {}
	}
}