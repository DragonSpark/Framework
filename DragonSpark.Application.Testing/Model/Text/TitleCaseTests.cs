using DragonSpark.Application.Model.Text;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Application.Testing.Model.Text;

public class TitleCaseTests
{
	[Fact]
	public void Verify()
	{
		TitleCase.Default.Get("Don`t laugh").Should().Be("Don't Laugh");
		TitleCase.Default.Get("Don‘t laugh").Should().Be("Don't Laugh");

	}
}