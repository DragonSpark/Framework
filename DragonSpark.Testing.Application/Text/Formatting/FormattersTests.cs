using System;
using FluentAssertions;
using DragonSpark.Text.Formatting;
using Xunit;

namespace DragonSpark.Testing.Application.Text.Formatting
{
	public sealed class FormattersTests
	{
		[Fact]
		void Verify()
		{
			ApplicationDomainFormatter.Default.Register();
			var sut = KnownFormatters.Default.Get(AppDomain.CurrentDomain);
			sut.ToString("F", FormatProvider.Default).Should().Be(AppDomain.CurrentDomain.FriendlyName);
			sut.ToString("I", FormatProvider.Default).Should().Be(AppDomain.CurrentDomain.Id.ToString());
			var @default = DefaultApplicationDomainFormatter.Default.Get(AppDomain.CurrentDomain);
			sut.ToString("x3", FormatProvider.Default).Should().Be(@default);
			sut.ToString("x3", null).Should().Be(@default);
			sut.ToString(string.Empty, null).Should().Be(@default);
			sut.ToString(null, null).Should().Be(@default);
		}
	}
}