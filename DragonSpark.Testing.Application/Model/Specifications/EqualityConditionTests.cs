using FluentAssertions;
using DragonSpark.Model.Selection.Conditions;
using Xunit;

namespace DragonSpark.Testing.Application.Model.Specifications
{
	public class EqualityConditionTests
	{
		[Fact]
		public void Number()
		{
			var sut = new EqualityCondition<int>(3);
			sut.Get(4)
			   .Should()
			   .BeFalse();
			sut.Get(3)
			   .Should()
			   .BeTrue();
		}

		[Fact]
		public void Object()
		{
			var source = new object();
			var sut    = new EqualityCondition<object>(source);
			sut.Get(new object())
			   .Should()
			   .BeFalse();
			sut.Get(source)
			   .Should()
			   .BeTrue();
		}
	}
}