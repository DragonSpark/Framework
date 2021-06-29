using DragonSpark.Compose;
using DragonSpark.Model.Sequences.Memory;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Model.Sequences.Memory
{
	public sealed class CombineLeasesTests
	{
		[Fact]
		public void Verify()
		{
			using var first = Leases<int>.Default.Get(3);
			first[0] = 1;
			first[1] = 2;
			first[2] = 3;

			using var second = Leases<int>.Default.Get(3);
			second[0] = 4;
			second[1] = 5;
			second[2] = 6;

			using var combined = CombineLeases<int>.Default.Get(first, second);
			first.ActualLength.Should().Be(16);
			combined.AsSpan().ToArray().Should().Equal(1, 2, 3, 4, 5, 6);
			combined.Length.Should().Be(6);
		}
	}
}