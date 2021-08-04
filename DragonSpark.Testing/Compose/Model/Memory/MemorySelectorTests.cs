using DragonSpark.Compose;
using FluentAssertions;
using System;
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
	}
}