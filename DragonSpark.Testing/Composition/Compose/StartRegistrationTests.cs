using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Results;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace DragonSpark.Testing.Composition.Compose
{
	// ReSharper disable once TestFileNameWarning
	public sealed class StartRegistrationTests
	{
		[Fact]
		public void Verify()
		{
			var collection = new ServiceCollection();
			var subject = collection.Start<ISubject>()
			                        .Forward<Subject>()
			                        .Scoped()
			                        .Then.BuildServiceProvider();

			subject.GetRequiredService<ISubject>()
			       .Should()
			       .BeOfType<Subject>();
		}

		[Fact]
		public void VerifyDependency()
		{
			var collection = new ServiceCollection();
			var subject = collection.Start<ISubject>()
			                        .Forward<SubjectWithDependency>()
			                        .Include(x => x.Dependencies)
			                        .Scoped()
			                        .Then.BuildServiceProvider();

			var service = subject.GetRequiredService<ISubject>();
			service.Should()
			       .BeOfType<SubjectWithDependency>();

			service.To<SubjectWithDependency>().Other.Should().NotBeNull();
		}

		[Fact]
		public void VerifyComposite()
		{
			var collection = new ServiceCollection();
			var subject = collection.Start<First>()
			                        .And<Second>()
			                        .And<Third>()
			                        .Include(x => x.Dependencies)
			                        .Scoped()
			                        .Then.BuildServiceProvider();
			subject.GetRequiredService<First>().Should().NotBeNull();
			subject.GetRequiredService<Second>().Should().NotBeNull();
			var third = subject.GetRequiredService<Third>();
			third.Should().NotBeNull();
			third.Other.Should().NotBeNull();
		}

		[Fact]
		public void VerifyDecoration()
		{
			var services = new ServiceCollection();
			var subject = services.Start<ISubject>()
			                      .Forward<Subject>()
			                      .Decorate<DecoratedSubject>()
			                      .Decorate<AnotherDecoratedSubject>()
			                      .Include(x => x.Dependencies)
			                      .Scoped()
			                      .Then.BuildServiceProvider();

			subject.GetRequiredService<ISubject>()
			       .Should()
			       .BeOfType<AnotherDecoratedSubject>()
			       .Subject.Inner.Should()
			       .BeOfType<DecoratedSubject>()
			       .Subject.Other.Should()
			       .NotBeNull();
		}

		[Fact]
		public void VerifyRecursiveDependenciesShouldFail()
		{
			var services = new ServiceCollection();
			var subject = services.Start<ISubject>()
			                      .Forward<Subject>()
			                      .Decorate<DecoratedSubjectForRecursion>()
			                      .Decorate<AnotherDecoratedSubject>()
			                      .Include(x => x.Dependencies)
			                      .Scoped()
			                      .Then.BuildServiceProvider();

			subject.Invoking(x => x.GetRequiredService<ISubject>()).Should().Throw<InvalidOperationException>();
		}

		[Fact]
		public void VerifyRecursiveDependencies()
		{
			var services = new ServiceCollection();
			var subject = services.Start<ISubject>()
			                      .Forward<Subject>()
			                      .Decorate<DecoratedSubjectForRecursion>()
			                      .Decorate<AnotherDecoratedSubject>()
			                      .Include(x => x.Dependencies.Recursive())
			                      .Scoped()
			                      .Then.BuildServiceProvider();

			subject.GetRequiredService<ISubject>()
			       .Should()
			       .BeOfType<AnotherDecoratedSubject>()
			       .Subject.Inner.Should()
			       .BeOfType<DecoratedSubjectForRecursion>()
			       .Subject.Inner.Should()
			       .NotBeNull();
		}

		[Fact]
		public void VerifyRecursiveDependenciesDeep()
		{
			var services = new ServiceCollection();
			var subject = services.Start<ISubject>()
			                      .Forward<Subject>()
			                      .Decorate<DecoratedSubjectForRecursionDeep>()
			                      .Decorate<AnotherDecoratedSubject>()
			                      .Include(x => x.Dependencies.Recursive())
			                      .Scoped()
			                      .Then.BuildServiceProvider();

			subject.GetRequiredService<ISubject>()
			       .Should()
			       .BeOfType<AnotherDecoratedSubject>()
			       .Subject.Inner.Should()
			       .BeOfType<DecoratedSubjectForRecursionDeep>()
			       .Subject.Inner.Inner.Inner.Should()
			       .NotBeNull();
		}

		[Fact]
		public void VerifyResult()
		{
			new ServiceCollection().Start<string>()
			                       .Use<MessageResult>()
			                       .Singleton()
			                       .Then.BuildServiceProvider()
			                       .GetRequiredService<string>()
			                       .Should()
			                       .Be(MessageResult.Default.Get());
		}

		sealed class MessageResult : Result<string>
		{
			public static MessageResult Default { get; } = new MessageResult();

			public MessageResult() : base(() => "Hello World!") {}
		}

		interface ISubject {}

		class Subject : ISubject {}

		class DecoratedSubject : ISubject
		{
			public DecoratedSubject(ISubject previous, Other other)
			{
				Previous = previous;
				Other    = other;
			}

			public ISubject Previous { [UsedImplicitly] get; }

			public Other Other { get; }
		}

		sealed class RecursiveOtherDeep
		{
			public RecursiveOtherDeep(RecursiveOther other) => Inner = other;

			public RecursiveOther Inner { [UsedImplicitly] get; }
		}

		sealed class RecursiveOther
		{
			public RecursiveOther(Other other) => Inner = other;

			public Other Inner { [UsedImplicitly] get; }
		}

		sealed class DecoratedSubjectForRecursionDeep : ISubject
		{
			public DecoratedSubjectForRecursionDeep(ISubject previous, RecursiveOtherDeep inner)
			{
				Previous = previous;
				Inner    = inner;
			}

			public ISubject Previous { [UsedImplicitly] get; }

			public RecursiveOtherDeep Inner { get; }
		}

		class DecoratedSubjectForRecursion : ISubject
		{
			public DecoratedSubjectForRecursion(ISubject previous, RecursiveOther inner)
			{
				Previous = previous;
				Inner    = inner;
			}

			public ISubject Previous { [UsedImplicitly] get; }

			public RecursiveOther Inner { get; }
		}

		class AnotherDecoratedSubject : ISubject
		{
			public AnotherDecoratedSubject(ISubject subject) => Inner = subject;

			public ISubject Inner { get; }
		}

		sealed class SubjectWithDependency : ISubject
		{
			public SubjectWithDependency(Other other) => Other = other;

			public Other Other { get; }
		}

		sealed class Other {}

		sealed class First {}

		sealed class Second {}

		sealed class Third
		{
			public Third(Other other) => Other = other;

			public Other Other { get; }
		}
	}
}