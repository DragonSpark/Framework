using DragonSpark.Model.Selection.Conditions;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Model.Specifications
{
	public class DecoratedConditionTests
	{
		[Fact]
		public void Verify()
		{
			new Condition<object>(Always<object>.Default).Get(new object()).Should().BeTrue();
		}
	}
}