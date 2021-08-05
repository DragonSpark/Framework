using DragonSpark.Compose;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Compose.Model.Memory
{
	public class MemorySelectorTests
	{
		[Fact]
		public void VerifyIndexOf()
		{
			var selection = new[] { 1, 2, 3, 4 };

			var sut = selection.AsMemory().Then();
			sut.IndexOf(3).Should().Be(2);
			sut.IndexOf(5).Should().BeNull();
		}

		[Fact]
		public void VerifyConcat()
		{
			var first  = new[] { 1, 2, 3, 4 };
			var second = new[] { 5, 6, 7, 8 };

			using var concat   = first.AsMemory().Then().Concat(second);
			var       span     = concat.AsSpan();
			var       expected = first.Concat(second).ToArray();
			span.ToArray().Should().Equal(expected);
			span.Length.Should().Be(expected.Length);
		}
	}
}