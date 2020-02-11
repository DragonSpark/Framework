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
		void Verify()
		{
			var instance = new Instance();

			Start.A.Result(instance)
			     .Return()
			     .Should()
			     .BeSameAs(instance);
		}

		[Fact]
		void VerifyActivation()
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
		void VerifyBasicExtent()
		{
			var instance = new Instance();

			Start.An.Extent<Instance>()
			     .Into.Result.Using(instance)
			     .Return()
			     .Should()
			     .BeSameAs(instance);
		}

		[Fact]
		void VerifyCalling()
		{
			var instance = new Instance();

			Start.A.Result.Of<Instance>()
			     .By.Calling(instance.Self)
			     .Return()
			     .Should()
			     .BeSameAs(instance);
		}
	}
}