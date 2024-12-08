using DragonSpark.Application.AspNet.Communication;
using DragonSpark.Application.Communication;
using DragonSpark.Compose;
using DragonSpark.Presentation.Environment;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace DragonSpark.Presentation.Testing.Environment;

public sealed class DomainAwareReferrerHeaderTests
{
	[Fact]
	public void Verify()
	{
		var sut = new DomainAwareReferrerHeader("domain.com");
		var context = new DefaultHttpContext
		{
			Request = { Scheme = "https", Host = new("some.domain.com") }
		};
		ReferrerHeader.Default.Assign(context.Request.Headers, "https://domain.com");
		var referrer = sut.Get(context.Request);
		referrer.Should().BeNull();
	}

	[Fact]
	public void VerifySubdomain()
	{
		var sut = new DomainAwareReferrerHeader("domain.com");
		var context = new DefaultHttpContext
		{
			Request = { Scheme = "https", Host = new("some.domain.com") }
		};
		ReferrerHeader.Default.Assign(context.Request.Headers, "https://some.domain.com");
		var referrer = sut.Get(context.Request);
		referrer.Should().BeNull();
	}

	[Fact]
	public void VerifyExternal()
	{
		var sut = new DomainAwareReferrerHeader("domain.com");
		var context = new DefaultHttpContext
		{
			Request = { Scheme = "https", Host = new("some.domain.com") }
		};
		const string expected = "https://some.other.com";
		ReferrerHeader.Default.Assign(context.Request.Headers, expected);
		var referrer = sut.Get(context.Request);
		referrer.Should().Be(expected);
	}
}