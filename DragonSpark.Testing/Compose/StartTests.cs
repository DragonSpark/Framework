using DragonSpark.Compose;
using FluentAssertions;
using JetBrains.Annotations;
using Xunit;

namespace DragonSpark.Testing.Compose
{
	public sealed class StartTests
	{
		sealed class Instance {}

		sealed class Singleton
		{
			public static Singleton Default { get; } = new Singleton();

			Singleton() {}
		}

		sealed class Activated
		{
			public static Activated Default { get; } = new Activated();

			[UsedImplicitly]
			public Activated() {}
		}

		[Fact]
		public void Verify()
		{
			var instance = new Instance();

			Start.A.Result(instance)
			     .Instance()
			     .Should()
			     .BeSameAs(instance);
		}

		[Fact]
		public void VerifyActivation()
		{
			var activation = Start.An.Activation<Instance>();
			activation.Default.Should()
			          .BeNull();

			activation.New()
			          .Should()
			          .NotBeNull();

			Start.An.Activation<Singleton>()
			     .Singleton()
			     .Should()
			     .BeSameAs(Singleton.Default);

			var activated = Start.An.Activation<Activated>();
			activated.Activate()
			         .Should()
			         .BeSameAs(Activated.Default);
			activated.New()
			         .Should()
			         .NotBeNull();
		}

		[Fact]
		public void VerifyBasicExtent()
		{
			var instance = new Instance();

			Start.An.Extent<Instance>()
			     .Into.Result.Using(instance)
			     .Instance()
			     .Should()
			     .BeSameAs(instance);
		}

		[Fact]
		public void VerifyCalling()
		{
			var instance = new Instance();

			Start.A.Result.Of<Instance>()
			     .By.Calling(instance.Self)
			     .Instance()
			     .Should()
			     .BeSameAs(instance);
		}

		sealed class Subject {}

		sealed class SingletonSubject
		{
			public static SingletonSubject Default { get; } = new SingletonSubject();

			SingletonSubject() {}
		}

		[Fact]
		public void VerifyArray()
		{
			Start.A.Result<Subject>()
			     .As.Sequence.Array.New(0)
			     .Instance()
			     .Should()
			     .BeEmpty();

			Start.A.Result<Subject>()
			     .As.Sequence.Array.New(4)
			     .Instance()
			     .Should()
			     .HaveCount(4);
		}

		[Fact]
		public void VerifyDefault()
		{
			Start.A.Result<Subject>()
			     .By.Default()
			     .Instance()
			     .Should()
			     .BeNull();
		}

		[Fact]
		public void VerifySingleton()
		{
			var source = Start.A.Result<SingletonSubject>().By.Activation();
			var first  = source.Instance();
			first.Should().Be(SingletonSubject.Default);
			source.Instance().Should().BeSameAs(first);
		}

		[Fact]
		public void VerifySubject()
		{
			var source = Start.A.Result<Subject>().By.Activation();
			var first  = source.Get();
			first.Should().NotBeNull();
			source.Instance().Should().NotBeNull().And.Subject.Should().NotBeSameAs(first);
		}
	}
}