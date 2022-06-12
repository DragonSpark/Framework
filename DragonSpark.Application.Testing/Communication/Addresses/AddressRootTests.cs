using DragonSpark.Application.Communication.Addresses;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Application.Testing.Communication.Addresses;

public sealed class AddressRootTests
{
	[Theory]
	[InlineData("some@email.com", "some@email.com")]
	[InlineData("some@somedomain.com", "some@somedomain.com")]
	[InlineData("somes.name@somedomain.com", "somes.name@somedomain.com")]
	[InlineData("somes.name+tagsareok@somedomain.com", "somes.name@somedomain.com")]
	public void Verify(string input, string expected)
	{
		AddressRoot.Default.Get(input).Should().Be(expected);
	}
}