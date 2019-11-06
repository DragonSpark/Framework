using FluentAssertions;
using Moq;
using DragonSpark.Model.Selection.Conditions;
using Xunit;

namespace DragonSpark.Testing.Application.Model.Specifications
{
	public class NeverTests
	{
		[Fact]
		public void Coverage()
		{
			Never.Default.Get(It.IsAny<object>())
			     .Should()
			     .BeFalse();
		}
	}
}