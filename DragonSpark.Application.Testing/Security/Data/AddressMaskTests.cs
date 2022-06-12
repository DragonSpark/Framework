using DragonSpark.Application.Communication.Addresses;
using FluentAssertions;
using System.Net.Mail;
using Xunit;

namespace DragonSpark.Application.Testing.Security.Data;

public sealed class AddressMaskTests
{
	[Theory]
	[InlineData("some@email.com", "s...e@e...l.com")]
	[InlineData("some@somedomain.com", "s...e@so...in.com")]
	[InlineData("somes.name@somedomain.com", "so...me@so...in.com")]
	[InlineData("somes.name+tagsareok@somedomain.com", "so...me+tagsareok@so...in.com")]
	public void Verify(string input, string expected)
	{
		AddressMask.Default.Get(new MailAddress(input)).Should().Be(expected);
	}
}