using DragonSpark.Application.Hosting.xUnit;
using DragonSpark.Compose;
using DragonSpark.Compose.Extents;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime.Activation;
using FluentAssertions;
using Xunit;
using Activator = DragonSpark.Runtime.Activation.Activator;

// ReSharper disable All

namespace DragonSpark.Testing.Application.Runtime.Activation
{
	// ReSharper disable once TestFileNameWarning
	public sealed class ActivateTests
	{
		[Theory, AutoData]
		public void VerifyNumber(int number)
		{
			var subject = Start.An.Extent<Subject>().From(number);
			subject.Should().NotBeSameAs(Start.An.Extent<Subject>().From(number));

			subject.Number.Should().Be(number);
		}

		[Theory, AutoData]
		void VerifyParameter(Extent<Object> sut, object parameter)
		{
			sut.New(parameter).Should().NotBeSameAs(sut.New(parameter));
		}

		[Theory, AutoFixture.Xunit2.AutoData]
		void VerifyActivateExtensionMethod(Subject<string, string> sut)
		{
			sut.Should().BeSameAs(Subject<string, string>.Default);
		}

		sealed class Activated {}

		sealed class Singleton
		{
			public static Singleton Default { get; } = new Singleton();

			Singleton() {}
		}

		sealed class Subject<TIn, TOut> : ISelect<TIn, TOut>
		{
			public static Subject<TIn, TOut> Default { get; } = new Subject<TIn, TOut>();

			Subject() {}

			public TOut Get(TIn parameter) => default!;
		}

		sealed class Subject : IActivateUsing<int>
		{
			public Subject(int number) => Number = number;

			public int Number { get; }
		}

		sealed class Object : IActivateUsing<object>
		{
			public Object(object @object) => O = @object;

			public object O { get; }
		}

		[Fact]
		public void VerifyGeneralized()
		{
			Activator.Default.Get(typeof(Activated))
			         .Should()
			         .NotBeNull()
			         .And.Subject.Should()
			         .BeOfType<Activated>()
			         .And.Subject.Should()
			         .NotBeSameAs(Activator.Default.Get(typeof(Activated)));
		}

		[Fact]
		public void VerifyGeneralizedSingleton()
		{
			Activator.Default.Get(typeof(Singleton)).Should().BeSameAs(Singleton.Default);
		}

		[Fact]
		public void VerifyGet()
		{
			Is.Assigned().Get().Get(Singleton.Default).Should().BeTrue();

			Activator<Singleton>.Default.Get().Should().BeSameAs(Singleton.Default);

			Activator<Singleton>.Default.Get().Should().BeSameAs(Activator<Singleton>.Default.Get());
		}

		[Fact]
		public void VerifyNew()
		{
			New<Activated>.Default.Get().Should().NotBeSameAs(New<Activated>.Default.Get());
		}
	}
}