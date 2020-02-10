using DragonSpark.Compose;
using DragonSpark.Composition.Compose;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace DragonSpark.Testing.Application.Composition
{
	public sealed class DependencyCandidatesTests
	{
		[Fact]
		void Verify()
		{
			DependencyCandidates.Default.Get(typeof(Subject))
			                    .Open()
			                    .Only()
			                    .Should()
			                    .Be(typeof(Other));
		}

		[Fact]
		void VerifyMultiple()
		{
			DependencyCandidates.Default.Get(typeof(Multiple))
			                    .Open()
			                    .Should()
			                    .Equal(typeof(Subject), typeof(Other));
		}

		[Fact]
		void VerifyMultipleWithInterface()
		{
			DependencyCandidates.Default.Get(typeof(MultipleWithInterface))
			                    .Open()
			                    .Should()
			                    .Equal(typeof(Subject), typeof(Other));
		}

		[Fact]
		void VerifyMultipleWithDelegate()
		{
			DependencyCandidates.Default.Get(typeof(MultipleWithDelegate))
			                    .Open()
			                    .Should()
			                    .Equal(typeof(Subject), typeof(Other));
		}


		[Fact]
		void VerifyMultipleGeneric()
		{
			DependencyCandidates.Default.Get(typeof(Multiple<object>))
			                    .Open()
			                    .Should()
			                    .Equal(typeof(Subject), typeof(Other), typeof(List<object>));
		}

		[Fact]
		void VerifyMultipleDefinition()
		{
			DependencyCandidates.Default.Get(typeof(Multiple<>))
			                    .Open()
			                    .Should()
			                    .Equal(typeof(Subject), typeof(Other), typeof(List<>));
		}

		sealed class Subject
		{
			public Subject(Other other) => Other = other;

			public Other Other { get; }
		}

		sealed class Other {}

		sealed class Multiple
		{
			public Multiple(Subject subject, Other other)
			{
				Subject = subject;
				Other   = other;
			}

			public Subject Subject { get; }

			public Other Other { get; }
		}

		sealed class MultipleWithInterface
		{
			public MultipleWithInterface(Subject subject, Other other, IConvertible convertible)
			{
				Subject     = subject;
				Other       = other;
				Convertible = convertible;
			}

			public Subject Subject { get; }

			public Other Other { get; }

			public IConvertible Convertible { get; }
		}

		sealed class MultipleWithDelegate
		{
			public MultipleWithDelegate(Subject subject, Other other, Func<object, object> @delegate)
			{
				Subject  = subject;
				Other    = other;
				Delegate = @delegate;
			}

			public Subject Subject { get; }

			public Other Other { get; }
			public Func<object, object> Delegate { get; }
		}

		sealed class Multiple<T>
		{
			public Multiple(Subject subject, Other other, List<T> list)
			{
				Subject = subject;
				Other   = other;
				List    = list;
			}

			public Subject Subject { get; }

			public Other Other { get; }
			public List<T> List { get; }
		}
	}
}