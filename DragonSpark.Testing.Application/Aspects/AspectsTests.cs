using FluentAssertions;
using DragonSpark.Aspects;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Xunit;

namespace DragonSpark.Testing.Application.Aspects
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
		void After()
		{
			AspectRegistry.Default.Get().Open().Should().BeEmpty();
		}

		[Fact]
		void Before()
		{
			AspectRegistry.Default.Get().Open().Should().BeEmpty();
		}

		[Fact]
		void Verify()
		{
			var self   = A.Self<object>();
			var aspect = Aspects<object, object>.Default.Get(self);
			aspect.Should().BeSameAs(Aspects<object, object>.Default.Get(self));
			aspect.Get(self).Should().BeSameAs(self);
		}

		[Fact]
		void VerifyCast()
		{
			Decoration<object, object>.Default.Registered();

			var self   = A.Self<string>();
			var aspect = Aspects<string, string>.Default.Get(self);
			aspect.Should().BeSameAs(Aspects<string, string>.Default.Get(self));
			aspect.Get(self).Should().BeOfType<Cast<object, object, string, string>.Container>();
		}

		[Fact]
		void VerifyRegistered()
		{
			AspectRegistry.Default.Get().Open().Should().BeEmpty();
			Decoration<object, object>.Default.Registered();
			AspectRegistry.Default.Get().Open().Should().NotBeEmpty();

			var self   = A.Self<object>();
			var aspect = Aspects<object, object>.Default.Get(self);
			aspect.Should().BeSameAs(Aspects<object, object>.Default.Get(self));
			aspect.Get(self).Should().BeOfType<Decorated<object, object>>();
		}

		[Fact]
		void VerifySpecific()
		{
			Decoration<object, string>.Default.Registered();

			var general     = Start.A.Selection.Of.Type<object>().By.Self;
			var generalized = Aspects<object, object>.Default.Get(general);
			generalized.Should().BeSameAs(Aspects<object, object>.Default.Get(general));
			generalized.Get(general).Should().BeSameAs(general);

			var different = Start.A.Selection.Of.Type<object>().AndOf<int>().By.Cast;
			var next      = Aspects<object, int>.Default.Get(different);
			next.Should().BeSameAs(Aspects<object, int>.Default.Get(different));
			next.Get(different).Should().BeSameAs(different);

			var specific = Start.A.Selection.Of.Type<object>().AndOf<string>().By.Cast;
			var aspect   = Aspects<object, string>.Default.Get(specific);
			aspect.Should().BeSameAs(Aspects<object, string>.Default.Get(specific));
			aspect.Get(specific)
			      .Should()
			      .NotBeSameAs(specific)
			      .And.Subject.Should()
			      .BeOfType<Decorated<object, string>>();
		}
	}
}