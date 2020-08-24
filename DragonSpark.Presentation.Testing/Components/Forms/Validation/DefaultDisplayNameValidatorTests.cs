using DragonSpark.Presentation.Components.Forms.Validation;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Presentation.Testing.Components.Forms.Validation
{
	public sealed class DefaultDisplayNameValidatorTests
	{
		[Fact]
		public void Verify()
		{
			var sut = DefaultDisplayNameValidator.Default;
			sut.Get("Valid Display Name").Should().BeTrue();
			sut.Get("Invalid $%^ ScreenName").Should().BeFalse();
			sut.Get("01234567890123456789012345678901234567890123456789").Should().BeTrue();
			sut.Get("012345678901234567890123456789012345678901234567891").Should().BeFalse();
		}
	}
}