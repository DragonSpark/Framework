using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Model.Selection.Conditions
{
	public class AnyConditionTests
	{
		[Fact]
		public void Is()
		{
			Always<object>.Default.Then()
			              .Or(Never<object>.Default)
			              .Get()
			              .Get(null!)
			              .Should()
			              .BeTrue();
		}

		[Fact]
		public void IsNot()
		{
			Never<object>.Default.Then()
			             .Or(Never<object>.Default)
			             .Get()
			             .Get(null!)
			             .Should()
			             .BeFalse();
		}
	}
}