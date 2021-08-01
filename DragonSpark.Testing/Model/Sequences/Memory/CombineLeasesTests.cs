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
			var       fSpan = first.AsSpan();
			fSpan[0] = 1;
			fSpan[1] = 2;
			fSpan[2] = 3;

			using var second = Leases<int>.Default.Get(3);
			var sSpan = second.AsSpan();
			sSpan[0] = 4;
			sSpan[1] = 5;
			sSpan[2] = 6;

			using var combined = CombineLeases<int>.Default.Get(first, second);
			first.ActualLength.Should().Be(16);
			combined.AsSpan().ToArray().Should().Equal(1, 2, 3, 4, 5, 6);
			combined.Length.Should().Be(6);
		}
	}
}