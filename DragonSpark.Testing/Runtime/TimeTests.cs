using DragonSpark.Runtime;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Runtime
{
	public sealed class TimeTests
	{
		[Fact]
		public void Verify()
		{
			var sut   = Time.Default;
			var ticks = sut.Get().Ticks;
			ticks.Should().Be(ticks);
			sut.Get().Ticks.Should().NotBe(sut.Get().Ticks);
		}
	}
}