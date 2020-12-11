using DragonSpark.Application.Compose.Entities.Generation;
using DragonSpark.Application.Hosting.xUnit;
using DragonSpark.Compose;
using FluentAssertions;
using JetBrains.Annotations;
using System;
using Xunit;

namespace DragonSpark.Application.Testing.Compose.Entities.Generation
{
	public sealed class GeneratorContextTests
	{
		[Fact]
		public void Verify()
		{
			Start.A.Generator<Basic.Parent>().Include(x => x.Child).Get().Child.Should().NotBeNull();
		}

		[Fact]
		public void VerifyNamed()
		{
			var parent = Start.A.Generator<Named.Parent>().Include(x => x.Child).Get();
			parent.Child.Parent.Should().NotBeNull().And.Subject.Should().BeSameAs(parent);
		}

		[Fact]
		public void VerifyMultiple()
		{
			Start.A.Generator<Multiple.Parent>()
			     .Invoking(x => x.Include(y => y.Child))
			     .Should()
			     .Throw<InvalidOperationException>();
		}

		[Fact]
		public void VerifyMultipleDirect()
		{
			var subject = Start.A.Generator<Multiple.Parent>()
			                   .Include(y => y.Child, child => child.Parent1)
			                   .Get();
			subject.Child.Parent1.Should().BeSameAs(subject);
			subject.Child.Parent2.Should().BeNull();
		}

		[Theory, AutoData]
		public void VerifyGenerate(Guid expected)
		{
			var generated = Start.A.Generator<Generate.Parent>()
			                     .Include(x => x.Child,
			                              x => x.Generate((faker, parent)
				                                              => faker.Generate().With(y => y.Id = expected)))
			                     .Get();
			generated.Child.Id.Should().Be(expected);
			generated.Child.Parent.Should().BeSameAs(generated);
		}

		[Fact]
		public void VerifyPost()
		{
			var @default = Start.A.Generator<PostConfigure.Parent>()
			                    .Include(x => x.Child)
			                    .Get();
			@default.Child.Count1.Should().NotBe(default);
			@default.Child.Count2.Should().NotBe(default);
			@default.Child.Count3.Should().NotBe(default);
			@default.Child.Parent.Should().BeSameAs(@default);

			{
				var post = Start.A.Generator<PostConfigure.Parent>()
				                .Include(x => x.Child,
				                         x => x.Configure((_, child) => child.Count1 = child.Count2 = child.Count3 = 0))
				                .Get();
				post.Child.Count1.Should().Be(default);
				post.Child.Count2.Should().Be(default);
				post.Child.Count3.Should().Be(default);
				post.Child.Parent.Should().BeSameAs(post);
			}

			{
				var post = Start.A.Generator<PostConfigure.Parent>()
				                .Include(x => x.Child,
				                         x => x.Configure((_, __, child)
					                                          => child.Count1 = child.Count2 = child.Count3 = 0))
				                .Get();
				post.Child.Count1.Should().Be(default);
				post.Child.Count2.Should().Be(default);
				post.Child.Count3.Should().Be(default);
				post.Child.Parent.Should().BeSameAs(post);
			}
		}

		[Fact]
		public void VerifyScoping()
		{
			var sut = Start.A.Generator<Basic.Parent>()
			               .Include(x => x.Child, include => include.Scoped(y => y.Once()));
			var first  = sut.Get();
			var second = sut.Get();

			first.Child.Should().BeSameAs(second.Child).And.Subject.Should().NotBeNull();
		}

		[Fact]
		public void VerifyScopingEach()
		{
			var sut = Start.A.Generator<Basic.Parent>()
			               .Include(x => x.Child, include => include.Scoped(y => y.PerCall()));
			var first  = sut.Get();
			var second = sut.Get();

			first.Child.Should().NotBeSameAs(second.Child).And.Subject.Should().NotBeNull();
		}

		static class Basic
		{
			public sealed class Parent
			{
				public Child Child { get; set; } = default!;
			}

			public sealed class Child {}
		}

		static class Multiple
		{
			public sealed class Parent
			{
				public Child Child { get; set; } = default!;
			}

			[UsedImplicitly]
			public sealed class Child
			{
				[UsedImplicitly]
				public Parent Parent1 { get; set; } = default!;

				[UsedImplicitly]
				public Parent Parent2 { get; set; } = default!;
			}
		}

		static class Named
		{
			public sealed class Parent
			{
				public Child Child { get; set; } = default!;
			}

			public sealed class Child
			{
				public Parent Parent { get; set; } = default!;

				[UsedImplicitly]
				public Parent Other { get; set; } = default!;
			}
		}

		static class Generate
		{
			public sealed class Parent
			{
				public Child Child { get; set; } = default!;
			}

			public sealed class Child
			{
				public Parent Parent { get; set; } = default!;

				[UsedImplicitly]
				public Guid Id { get; set; }
			}
		}

		static class PostConfigure
		{
			public sealed class Parent
			{
				public Child Child { get; set; } = default!;
			}

			public sealed class Child
			{
				public Parent Parent { get; set; } = default!;

				[UsedImplicitly]
				public Guid Id { get; set; }

				public uint Count1 { get; set; }

				public uint Count2 { get; set; }

				public uint Count3 { get; set; }
			}
		}
	}
}