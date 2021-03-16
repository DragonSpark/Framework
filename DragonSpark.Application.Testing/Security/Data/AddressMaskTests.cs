using DragonSpark.Application.Security.Data;
using FluentAssertions;
using System.Net.Mail;
using Xunit;

namespace DragonSpark.Application.Testing.Security.Data
{
	public sealed class AddressMaskTests
	{
		[Fact]
		public void Verify()
		{
			AddressMask.Default.Get(new MailAddress("some@email.com")).Should().Be("s...e@e...l.com");
			AddressMask.Default.Get(new MailAddress("some@somedomain.com")).Should().Be("s...e@so...in.com");
			AddressMask.Default.Get(new MailAddress("somes.name@somedomain.com"))
			           .Should()
			           .Be("so...me@so...in.com");
		}
	}
}