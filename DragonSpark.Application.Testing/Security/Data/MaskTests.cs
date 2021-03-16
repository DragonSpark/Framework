using DragonSpark.Application.Security.Data;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Application.Testing.Security.Data
{
	public class MaskTests
	{
		[Fact]
		public void Verify()
		{
			Mask.Default.Get("user1").Should().Be("u...1");
			Mask.Default.Get("first.last").Should().Be("fi...st");
			Mask.Default.Get("me").Should().Be("**");
			Mask.Default.Get("o").Should().Be("*");
			Mask.Default.Get("tre").Should().Be("t...e");
		}
	}
}