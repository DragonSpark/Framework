using DragonSpark.Application.Security.Data;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Application.Testing.Security.Data
{
	public sealed class AddressMaskTests
	{
		[Fact]
		public void Verify()
		{
			AddressMask.Default.Get("some@email.com").Should().Be("s..e@e...l.com");
			AddressMask.Default.Get("some@somedomain.com").Should().Be("s..e@so......in.com");
			AddressMask.Default.Get("somes.name@somedomain.com").Should().Be("so......me@so......in.com");

		}
	}
}