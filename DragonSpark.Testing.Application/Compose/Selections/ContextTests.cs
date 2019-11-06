using FluentAssertions;
using DragonSpark.Compose;
using Xunit;

namespace DragonSpark.Testing.Application.Compose.Selections
{
	public sealed class ContextTests
	{
		sealed class Subject {}

		[Fact]
		void VerifyFull()
		{
			var instance  = new Subject();
			var parameter = new object();
			var result    = new Subject();

			var subject = Start.A.Selection.Of.Any.AndOf<Subject>().By.Cast.Or.Return(result);
			subject.Get(parameter).Should().BeSameAs(result);
			subject.Get(instance).Should().BeSameAs(instance);
		}

		[Fact]
		void VerifyParameter()
		{
			Start.A.Selection.Of.Any.By.Returning(4)
			     .Get(new object())
			     .Should()
			     .Be(4);
		}
	}
}