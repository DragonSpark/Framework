﻿using DragonSpark.Compose;
using FluentAssertions;
using System;
using Xunit;

namespace DragonSpark.Testing.Application.Compose.Extents.Results
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
		void VerifyActivation()
		{
			Start.A.Result<Subject>()
			     .By.Activation()
			     .Return()
			     .Should()
			     .NotBeNull();

			Start.A.Result<SubjectSingleton>()
			     .By.Activation()
			     .Return()
			     .Should()
			     .BeSameAs(SubjectSingleton.Default);
		}

		[Fact]
		void VerifyExtentSelection()
		{
			var instance = Array.Empty<Subject>();
			Start.A.Result<Subject>()
			     .As.Sequence.Array.By.Using(instance)
			     .Return()
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
			result.Return().Should().NotBeSameAs(instance);
		}

		[Fact]
		void VerifySingleton()
		{
			Start.A.Result.Of<SubjectSingleton>()
			     .By.Singleton()
			     .Return()
			     .Should()
			     .BeSameAs(SubjectSingleton.Default);
		}
	}
}