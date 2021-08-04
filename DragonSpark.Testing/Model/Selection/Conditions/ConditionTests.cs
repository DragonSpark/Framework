using DragonSpark.Model.Selection.Conditions;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Model.Selection.Conditions
{
	public class ConditionTests
	{
		[Fact]
		public void Verify()
		{
			new Condition<object>(Always<object>.Default).Get(new object()).Should().BeTrue();
		}
	}
}