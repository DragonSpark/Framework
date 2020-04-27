using DragonSpark.Compose;
using DragonSpark.Text.Formatting;
using FluentAssertions;
using System;
using Xunit;

namespace DragonSpark.Testing.Text.Formatting
{
	public sealed class FormattersTests
	{
		[Fact]
		public void Verify()
		{
			var formatter = DefaultSystemFormatter.Default.Append(ApplicationDomainFormatter.Default);
			var sut       = formatter.Get(AppDomain.CurrentDomain);
			var provider  = new FormatProvider(formatter);
			sut.ToString("F", provider).Should().Be(AppDomain.CurrentDomain.FriendlyName);
			sut.ToString("I", provider).Should().Be(AppDomain.CurrentDomain.Id.ToString());
			var @default = DefaultApplicationDomainFormatter.Default.Get(AppDomain.CurrentDomain);
			sut.ToString("x3", provider).Should().Be(@default);
			sut.ToString("x3", null).Should().Be(@default);
			sut.ToString(string.Empty, null).Should().Be(@default);
			sut.ToString(null, null).Should().Be(@default);
		}
	}
}