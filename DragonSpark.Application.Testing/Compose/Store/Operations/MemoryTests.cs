using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using Xunit;

namespace DragonSpark.Application.Testing.Compose.Store.Operations
{
	public sealed class MemoryTests
	{
		[Fact]
		public async Task Verify()
		{
			var memory = new MemoryCache(new MemoryCacheOptions());
			var selector = Start.A.Selection<None>()
			                    .By.Calling(new Count())
			                    .Then()
			                    .Store()
			                    .In(memory)
			                    .UntilRemoved()
			                    .Using(_ => "UniqueKey")
			                    .Protecting()
			                    .Allocate()
			                    .Get();

			var first  = selector.Get();
			var second = selector.Get();
			var third  = selector.Get();

			await Task.WhenAll(first, second, third);

			first.Result.Should()
			     .Be(second.Result)
			     .And.Subject.Should()
			     .Be(third.Result)
			     .And.Subject.Should()
			     .Be(1);
		}

		sealed class Count : IResulting<int>
		{
			int state;

			public async ValueTask<int> Get()
			{
				await Task.Delay(10); // Not as extreme as 3000 as to impact test suite.
				state++;
				return state;
			}
		}
	}
}