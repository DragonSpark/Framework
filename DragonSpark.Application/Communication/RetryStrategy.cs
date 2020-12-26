using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Application.Communication
{
	sealed class RetryStrategy : Select<int, TimeSpan>
	{
		public static RetryStrategy Default { get; } = new RetryStrategy();

		RetryStrategy() : base(count => TimeSpan.FromSeconds(Math.Pow(2, count))) {}
	}
}