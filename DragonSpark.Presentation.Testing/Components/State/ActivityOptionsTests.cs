using DragonSpark.Presentation.Components.State;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Presentation.Testing.Components.State;

public sealed class ActivityOptionsTests
{
	[Fact]
	public void Verify()
	{
		ActivityOptions.Default.RedrawOnFinish.Should().Be(false);
	}
}