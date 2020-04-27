using DragonSpark.Model.Selection.Conditions;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Model.Selection.Conditions
{
	public class EqualsTests
	{
		[Fact]
		public void Number()
		{
			var sut = new Equals<int>(3);
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
			var sut    = new Equals<object>(source);
			sut.Get(new object())
			   .Should()
			   .BeFalse();
			sut.Get(source)
			   .Should()
			   .BeTrue();
		}
	}
}