using DeviceDetectorNET.Parser;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Presentation.Testing;

public sealed class IsBotTests
{
	[Fact]
	public void Verify()
	{
		var sut = new BotParser { DiscardDetails = true };
		sut.SetUserAgent("Twitterbot/1.0");
		sut.Parse().Success.Should().BeTrue();
	}

	[Fact]
	public void VerifyNot()
	{
		var sut = new BotParser { DiscardDetails = true };
		sut.SetUserAgent("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.102 Safari/537.36");
		sut.Parse().Success.Should().BeFalse();
	}
}