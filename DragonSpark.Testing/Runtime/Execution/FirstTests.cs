using DragonSpark.Application.Hosting.xUnit;
using DragonSpark.Compose;
using DragonSpark.Runtime.Execution;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Runtime.Execution
{
	public sealed class FirstTests
	{
		[Theory, AutoDataModest]
		void VerifyFirst(First sut)
		{
			sut.Get().Should().BeTrue();
			sut.Get().Should().BeFalse();
		}

		[Theory, AutoData]
		void VerifyFirstReference(First<object> sut, object first, object second)
		{
			sut.Get(first).Should().BeTrue();
			sut.Get(first).Should().BeFalse();

			sut.Get(second).Should().BeTrue();
			sut.Get(second).Should().BeFalse();
		}

		[Theory, AutoData]
		void VerifyFirstEquality(First<int> sut, int first, int second)
		{
			sut.Get(first).Should().BeTrue();
			sut.Get(first).Should().BeFalse();

			sut.Get(second).Should().BeTrue();
			sut.Get(second).Should().BeFalse();
		}
	}
}