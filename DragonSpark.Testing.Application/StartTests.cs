using DragonSpark.Compose;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Application
{
	public sealed class StartTests
	{
		sealed class Subject {}

		sealed class SingletonSubject
		{
			public static SingletonSubject Default { get; } = new SingletonSubject();

			SingletonSubject() {}
		}

		[Fact]
		void VerifyArray()
		{
			Start.A.Result<Subject>()
			     .As.Sequence.Array.New(0)
			     .Return()
			     .Should()
			     .BeEmpty();

			Start.A.Result<Subject>()
			     .As.Sequence.Array.New(4)
			     .Return()
			     .Should()
			     .HaveCount(4);
		}

		[Fact]
		void VerifyDefault()
		{
			Start.A.Result<Subject>()
			     .By.Default()
			     .Return()
			     .Should()
			     .BeNull();
		}

		[Fact]
		void VerifySingleton()
		{
			var source = Start.A.Result<SingletonSubject>().By.Activation();
			var first  = source.Return();
			first.Should().Be(SingletonSubject.Default);
			source.Return().Should().BeSameAs(first);
		}

		[Fact]
		void VerifySubject()
		{
			var source = Start.A.Result<Subject>().By.Activation();
			var first  = source.Get();
			first.Should().NotBeNull();
			source.Return().Should().NotBeNull().And.Subject.Should().NotBeSameAs(first);
		}
	}
}