using System;
using FluentAssertions;
using DragonSpark.Compose;
using DragonSpark.Testing.Objects;
using Xunit;

namespace DragonSpark.Testing.Application.Compose.Results
{
	public sealed class ContextTests
	{
		sealed class Subject {}

		sealed class SubjectSingleton
		{
			public static SubjectSingleton Default { get; } = new SubjectSingleton();

			SubjectSingleton() {}
		}

		[Fact]
		void VerifyActivation()
		{
			Start.A.Result<Subject>()
			     .By.Activation()
			     .Get()
			     .Should()
			     .NotBeNull();

			Start.A.Result<SubjectSingleton>()
			     .By.Activation()
			     .Get()
			     .Should()
			     .BeSameAs(SubjectSingleton.Default);
		}

		[Fact]
		void VerifyExtentSelection()
		{
			var instance = Array.Empty<Subject>();
			Start.A.Result<Subject>()
			     .As.Sequence.Array.By.Using(instance)
			     .Get()
			     .Should()
			     .BeSameAs(instance);
		}

		[Fact]
		void VerifyLocation()
		{
			Start.A.Result<IHelloWorld>()
			     .By.Location.Get()
			     .Should()
			     .NotBeNull();
		}

		[Fact]
		void VerifyLocationDefault()
		{
			Start.A.Result<int>()
			     .By.Location
			     .Get()
			     .Should()
			     .Be(0);

			Start.A.Result<int>()
			     .By.Location.Or.Default(4)
			     .Get()
			     .Should()
			     .Be(4);
		}

		[Fact]
		void VerifyLocationOrThrow()
		{
			Start.A.Result.Of<int>()
			     .By.Location.Or.Throw()
			     .Invoking(x => x.Get())
			     .Should()
			     .Throw<InvalidOperationException>();
		}

		[Fact]
		void VerifyNew()
		{
			var result = Start.A.Result<Subject>()
			                  .By.Instantiation();
			var instance = result.Get();
			instance.Should()
			        .NotBeNull();
			result.Get().Should().NotBeSameAs(instance);
		}

		[Fact]
		void VerifySingleton()
		{
			Start.A.Result.Of<SubjectSingleton>()
			     .By.Singleton()
			     .Get()
			     .Should()
			     .BeSameAs(SubjectSingleton.Default);
		}
	}
}