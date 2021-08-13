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

		[Theory, AutoData]
		public void VerifyConfigure(DateTimeOffset expected)
		{
			var sut = Start.A.Generator<Configured.Parent>()
			               .Include(x => x.Child)
			               .Configure(x => x.Created, _ => expected)
			               .Get();
			sut.Child.Parent.Should().BeSameAs(sut);
			sut.Created.Should().Be(expected);
			sut.Id.Should().NotBeEmpty();
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
			                              x => x.Generate((faker, _)
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
				                         x => x.Configure((_, _, child)
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

		[Fact]
		public void VerifyThenInclude()
		{
			var sut = Start.A.Generator<ThenInclude.Parent>().Include(x => x.Child).ThenInclude(x => x.Other).Get();
			sut.Child.Other.Child.Should().BeSameAs(sut.Child);
		}

		[Fact]
		public void VerifyThenIncludeDirect()
		{
			var sut = Start.A.Generator<ThenIncludeMulti.Parent>()
			               .Include(x => x.Child)
			               .ThenInclude(x => x.Other, x => x.Child2)
			               .Get();
			sut.Child.Other.Child1.Should().BeNull();
			sut.Child.Other.Child2.Should().BeSameAs(sut.Child);
		}

		[Fact]
		public void VerifyThenIncludeDouble()
		{
			var sut = Start.A.Generator<ThenIncludeDouble.Parent>()
			               .Include(x => x.Child)
			               .ThenInclude(x => x.Other)
			               .ThenInclude(x => x.Item)
			               .Get();
			sut.Child.Other.Item.Should().NotBeNull();
			sut.Child.Other.Item.Other.Should().BeSameAs(sut.Child.Other);
		}

		[Fact]
		public void VerifyPivot()
		{
			var sut = Start.A.Generator<Pivot.Parent>()
			               .Include(x => x.Child)
			               .Pivot()
			               .Include(x => x.Other)
			               .Include(x => x.Item)
			               .Get();
			sut.Child.Other.Child.Should().BeSameAs(sut.Child.Item.Child).And.Subject.Should().NotBeNull();
		}

		[Fact]
		public void VerifyPivotThenInclude()
		{
			var sut = Start.A.Generator<PivotThenInclude.Parent>()
			               .Include(x => x.Child)
			               .Pivot()
			               .Include(x => x.Other)
			               .ThenInclude(x => x.Item)
			               .Get();
			sut.Child.Other.Item.Other.Should().BeSameAs(sut.Child.Other).And.Subject.Should().NotBeNull();
		}

		[Fact]
		public void VerifyPivotThenIncludeDirect()
		{
			var sut = Start.A.Generator<PivotThenIncludeDirect.Parent>()
			               .Include(x => x.Child)
			               .Pivot()
			               .Include(x => x.Other)
			               .ThenInclude(x => x.Item, x => x.Other2)
			               .Get();
			sut.Child.Other.Item.Other2.Should().BeSameAs(sut.Child.Other).And.Subject.Should().NotBeNull();
			sut.Child.Other.Item.Other1.Should().BeNull();
		}

		[Fact]
		public void VerifyExistingEncounter()
		{
			var sut = Start.A.Generator<Existing.Parent>()
			               .Include(x => x.Child)
			               .ThenInclude(x => x.Next)
			               .ThenInclude(x => x.Parent)
			               .Get();
			sut.Child.Next.Parent.Should().BeSameAs(sut);
		}

		[Fact]
		public void VerifyBasicDouble()
		{
			var sut = Start.A.Generator<BasicDouble.Parent>().Include(x => x.Child).Include(x => x.Next).Get();
			sut.Child.Parent.Should().BeSameAs(sut);
			sut.Next.Parent.Should().BeSameAs(sut);

		}

		static class BasicDouble
		{
			public sealed class Parent
			{
				public Child Child { get; set; } = default!;

				public Next Next { get; set; } = default!;
			}

			public sealed class Child
			{
				public Parent Parent { get; set; } = default!;
			}

			public sealed class Next
			{
				public Parent Parent { get; set; } = default!;
			}
		}

		static class Existing
		{
			public sealed class Parent
			{
				public Child Child { get; set; } = default!;
			}

			public sealed class Child
			{
				public Next Next { get; set; } = default!;
			}

			public sealed class Next
			{
				public Parent Parent { get; set; } = default!;
			}
		}

		static class Basic
		{
			public sealed class Parent
			{
				public Child Child { get; set; } = default!;
			}

			public sealed class Child {}
		}

		static class Configured
		{
			public sealed class Parent
			{
				[UsedImplicitly]
				public Guid Id { get; set; }

				[UsedImplicitly]
				public DateTimeOffset Created { get; set; }

				public Child Child { get; set; } = default!;
			}

			public sealed class Child
			{
				public Parent Parent { get; set; } = default!;
			}
		}

		static class ThenInclude
		{
			public sealed class Parent
			{
				public Child Child { get; set; } = default!;
			}

			public sealed class Child
			{
				[UsedImplicitly]
				public Parent Parent { get; set; } = default!;

				public Other Other { get; set; } = default!;
			}

			public sealed class Other
			{
				public Child Child { get; set; } = default!;
			}
		}

		static class Pivot
		{
			public sealed class Parent
			{
				public Child Child { get; set; } = default!;
			}

			public sealed class Child
			{
				[UsedImplicitly]
				public Parent Parent { get; set; } = default!;

				public Other Other { get; set; } = default!;

				public Item Item { get; set; } = default!;
			}

			public sealed class Other
			{
				public Child Child { get; set; } = default!;
			}

			public sealed class Item
			{
				public Child Child { get; set; } = default!;
			}
		}

		static class PivotThenInclude
		{
			public sealed class Parent
			{
				public Child Child { get; set; } = default!;
			}

			public sealed class Child
			{
				[UsedImplicitly]
				public Parent Parent { get; set; } = default!;

				public Other Other { get; set; } = default!;
			}

			public sealed class Other
			{
				public Item Item { get; set; } = default!;
			}

			public sealed class Item
			{
				public Other Other { get; set; } = default!;
			}
		}

		static class PivotThenIncludeDirect
		{
			public sealed class Parent
			{
				public Child Child { get; set; } = default!;
			}

			public sealed class Child
			{
				[UsedImplicitly]
				public Parent Parent { get; set; } = default!;

				public Other Other { get; set; } = default!;
			}

			public sealed class Other
			{
				public Item Item { get; set; } = default!;
			}

			public sealed class Item
			{
				public Other Other1 { get; set; } = default!;
				public Other Other2 { get; set; } = default!;
			}
		}

		static class ThenIncludeDouble
		{
			public sealed class Parent
			{
				public Child Child { get; set; } = default!;
			}

			public sealed class Child
			{
				[UsedImplicitly]
				public Parent Parent { get; set; } = default!;

				public Other Other { get; set; } = default!;
			}

			public sealed class Other
			{
				public Item Item { get; set; } = default!;
			}

			public sealed class Item
			{
				public Other Other { get; set; } = default!;
			}
		}

		static class ThenIncludeMulti
		{
			public sealed class Parent
			{
				public Child Child { get; set; } = default!;
			}

			public sealed class Child
			{
				[UsedImplicitly]
				public Parent Parent { get; set; } = default!;

				public Other Other { get; set; } = default!;
			}

			public sealed class Other
			{
				public Child Child1 { get; set; } = default!;

				public Child Child2 { get; set; } = default!;
			}
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