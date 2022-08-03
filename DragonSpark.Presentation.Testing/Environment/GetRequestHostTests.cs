using DragonSpark.Presentation.Environment;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace DragonSpark.Presentation.Testing.Environment;

public sealed class GetRequestHostTests
{
	[Fact]
	public void Verify()
	{
		var sut = GetRequestHost.Default;
		var context = new DefaultHttpContext
		{
			Request = { Scheme = "https", Host = new("some.domain.com") }
		};
		var host = sut.Get(context.Request);
		host.Should().Be("domain.com");
	}
}