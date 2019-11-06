using FluentAssertions;
using DragonSpark.Model.Selection.Conditions;
using Xunit;

namespace DragonSpark.Testing.Application.Model.Specifications
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