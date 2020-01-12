using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Application.Compose.Extents.Conditions
{
	public sealed class ConditionContextTests
	{
		[Fact]
		void Verify()
		{
			Start.A.Condition.Of.Any.By.Always.Get().Should()
			     .BeSameAs(Always<object>.Default);
		}

		[Fact]
		void VerifyUsing()
		{
			var subject = Start.A.Condition.Of.Type<int>().By.Calling(x => x > 3).Get();

			subject.Get(0).Should().BeFalse();
			subject.Get(3).Should().BeFalse();
			subject.Get(4).Should().BeTrue();
		}
	}
}