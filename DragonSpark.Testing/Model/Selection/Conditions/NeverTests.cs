using DragonSpark.Model.Selection.Conditions;
using FluentAssertions;
using Moq;
using Xunit;

namespace DragonSpark.Testing.Model.Selection.Conditions
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