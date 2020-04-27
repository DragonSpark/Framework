using DragonSpark.Compose;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Compose.Extents.Selections
{
	public sealed class SelectionContextTests
	{
		sealed class Subject {}

		[Fact]
		public void VerifyFull()
		{
			var instance  = new Subject();
			var parameter = new object();
			var result    = new Subject();

			var subject = Start.A.Selection.Of.Any.AndOf<Subject>().By.Cast.Or.Return(result).Get();
			subject.Get(parameter).Should().BeSameAs(result);
			subject.Get(instance).Should().BeSameAs(instance);
		}

		[Fact]
		public void VerifyParameter()
		{
			Start.A.Selection.Of.Any.By.Returning(4)
			     .Get()
			     .Get(new object())
			     .Should()
			     .Be(4);
		}
	}
}