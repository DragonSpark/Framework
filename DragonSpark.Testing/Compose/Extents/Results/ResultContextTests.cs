using DragonSpark.Compose;
using FluentAssertions;
using System;
using Xunit;

namespace DragonSpark.Testing.Compose.Extents.Results
{
	public sealed class ResultContextTests
	{
		sealed class Subject {}

		sealed class SubjectSingleton
		{
			public static SubjectSingleton Default { get; } = new SubjectSingleton();

			SubjectSingleton() {}
		}

		[Fact]
		public void VerifyActivation()
		{
			Start.A.Result<Subject>()
			     .By.Activation()
			     .Instance()
			     .Should()
			     .NotBeNull();

			Start.A.Result<SubjectSingleton>()
			     .By.Activation()
			     .Instance()
			     .Should()
			     .BeSameAs(SubjectSingleton.Default);
		}

		[Fact]
		public void VerifyExtentSelection()
		{
			var instance = Array.Empty<Subject>();
			Start.A.Result<Subject>()
			     .As.Sequence.Array.By.Using(instance)
			     .Instance()
			     .Should()
			     .BeSameAs(instance);
		}

		[Fact]
		public void VerifyNew()
		{
			var result = Start.A.Result<Subject>()
			                  .By.Instantiation();
			var instance = result.Get();
			instance.Should()
			        .NotBeNull();
			result.Instance().Should().NotBeSameAs(instance);
		}

		[Fact]
		public void VerifySingleton()
		{
			Start.A.Result.Of<SubjectSingleton>()
			     .By.Singleton()
			     .Instance()
			     .Should()
			     .BeSameAs(SubjectSingleton.Default);
		}
	}
}