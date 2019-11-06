using FluentAssertions;
using DragonSpark.Application.Hosting.xUnit;
using DragonSpark.Compose;
using DragonSpark.Runtime.Execution;
using Xunit;

namespace DragonSpark.Testing.Application.Runtime.Execution
{
	public sealed class CounterTests
	{
		[Theory, AutoDataModest]
		void Verify(Counter sut)
		{
			for (var i = 0; i < 100; i++)
			{
				sut.Count().Should().Be(i + 1);
			}
		}

		[Theory, AutoData]
		void VerifyReference(Counter<object> sut, object first, object second)
		{
			for (var i = 0; i < 100; i++)
			{
				sut.Get(first).Should().Be(i + 1);
			}

			for (var i = 0; i < 100; i++)
			{
				sut.Get(second).Should().Be(i + 1);
			}
		}

		[Theory, AutoData]
		void VerifyEquality(Counter<int> sut, int first, int second)
		{
			for (var i = 0; i < 100; i++)
			{
				sut.Get(first).Should().Be(i + 1);
			}

			for (var i = 0; i < 100; i++)
			{
				sut.Get(second).Should().Be(i + 1);
			}
		}

		[Fact]
		void VerifyBasic()
		{
			Start.A.Selection<int>().AndOf<Counter>().By.Instantiation.Get(1);
		}
	}
}