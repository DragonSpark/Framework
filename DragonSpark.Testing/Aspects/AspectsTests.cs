using DragonSpark.Compose;
using DragonSpark.Model.Aspects;
using DragonSpark.Model.Selection;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Aspects
{
	public sealed class AspectsTests
	{
		sealed class Decorated<TIn, TOut> : ISelect<TIn, TOut>
		{
			readonly ISelect<TIn, TOut> _select;

			public Decorated(ISelect<TIn, TOut> select) => _select = select;

			public TOut Get(TIn parameter) => _select.Get(parameter);
		}

		sealed class Decoration<TIn, TOut> : IAspect<TIn, TOut>
		{
			public static Decoration<TIn, TOut> Default { get; } = new Decoration<TIn, TOut>();

			Decoration() {}

			public ISelect<TIn, TOut> Get(ISelect<TIn, TOut> parameter) => new Decorated<TIn, TOut>(parameter);
		}

		[Fact]
		void Verify()
		{
			var self    = A.Self<object>();
			var aspects = new Aspects<object, object>();
			var aspect  = aspects.Get(self);
			aspect.Should().BeSameAs(aspects.Get(self));
			aspect.Get(self).Should().BeSameAs(self);
		}

		[Fact]
		void VerifyCast()
		{
			var registry = new AspectRegistry();
			registry.Registered(Decoration<object, object>.Default);

			var self    = A.Self<string>();
			var aspects = new Aspects<string, string>(registry);
			var aspect  = aspects.Get(self);
			aspect.Should().BeSameAs(aspects.Get(self));
			aspect.Get(self).Should().BeOfType<Cast<object, object, string, string>.Container>();
		}

		[Fact]
		void VerifyRegistered()
		{
			var registry = new AspectRegistry();

			registry.Get().Open().Should().BeEmpty();
			registry.Registered(Decoration<object, object>.Default);
			registry.Get().Open().Should().NotBeEmpty();

			var self    = A.Self<object>();
			var aspects = new Aspects<object, object>(registry);
			var aspect  = aspects.Get(self);
			aspect.Should().BeSameAs(aspects.Get(self));
			aspect.Get(self).Should().BeOfType<Decorated<object, object>>();
		}

		[Fact]
		void VerifySpecific()
		{
			var registry = new AspectRegistry();
			registry.Registered(Decoration<object, string>.Default);

			{
				var general     = Start.A.Selection.Of.Type<object>().By.Self.Get();
				var aspects     = new Aspects<object, object>(registry);
				var generalized = aspects.Get(general);
				generalized.Should().BeSameAs(aspects.Get(general));
				generalized.Get(general).Should().BeSameAs(general);
			}

			{
				var different = Start.A.Selection.Of.Type<object>().AndOf<int>().By.Cast;
				var aspects   = new Aspects<object, int>(registry);
				var next      = aspects.Get(different);
				next.Should().BeSameAs(aspects.Get(different));
				next.Get(different).Should().BeSameAs(different);
			}

			{
				var specific = Start.A.Selection.Of.Type<object>().AndOf<string>().By.Cast;
				var aspects  = new Aspects<object, string>(registry);
				var aspect   = aspects.Get(specific);
				aspect.Should().BeSameAs(aspects.Get(specific));
				aspect.Get(specific)
				      .Should()
				      .NotBeSameAs(specific)
				      .And.Subject.Should()
				      .BeOfType<Decorated<object, string>>();
			}
		}
	}
}