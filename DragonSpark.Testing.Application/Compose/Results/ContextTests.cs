using DragonSpark.Compose;
using FluentAssertions;
using System;
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