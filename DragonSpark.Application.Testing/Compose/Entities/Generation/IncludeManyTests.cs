using DragonSpark.Compose;
using FluentAssertions;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DragonSpark.Application.Testing.Compose.Entities.Generation
{
	public class IncludeManyTests
	{
		[Fact]
		public void Verify()
		{
			var subject = Start.A.Generator<Basic.Subject>().Include(x => x.Items).Get();
			subject.Items.Should().HaveCount(3);
			subject.Items.Distinct().Should().HaveCount(3);
			subject.Items.Select(x => x.Name).Distinct().Should().HaveCount(3);
			subject.Items.Select(x => x.Subject).Distinct().Only().Should().BeSameAs(subject);
		}

		[Fact]
		public void VerifyEmpty()
		{
			var subject = Start.A.Generator<Basic.Subject>()
			                   .Include(x => x.Items, x => x.Empty())
			                   .Get();
			subject.Items.Should().BeEmpty();
		}

		[Fact]
		public void VerifyComplex()
		{
			var subject = Start.A.Generator<Complex.Subject>()
			                   .Include(x => x.Items)
			                   .Pivot()
			                   .Include(x => x.Others)
			                   .ThenInclude(x => x.Subject)
			                   .Get();
			subject.Items.Should().HaveCount(3);
			subject.Items.First().Others.Should().HaveCount(3);
			subject.Items.First().Others.Select(x => x.Subject).Distinct().Only().Should().BeSameAs(subject);
		}

		[Fact]
		public void VerifySuperComplex()
		{
			var subject = Start.A.Generator<SuperComplex.Subject>()
			                   .Include(x => x.Items)
			                   .Pivot()
			                   .Include(x => x.Others)
			                   .ThenInclude(x => x.Another)
			                   .ThenInclude(x => x.YetAnother, include => include.Another2)
			                   .Get();
			subject.Items.Should().HaveCount(3);
			subject.Items.First().Others.Should().HaveCount(3);
			var another = subject.Items.First().Others.First().Another.YetAnother;
			another.Another2.Should().NotBeNull();
			another.Another2.YetAnother.Should().BeSameAs(another);
			another.Another1.Should().BeNull();
		}

		[Fact]
		public void VerifyAnotherSuperComplex()
		{
			var subject = Start.A.Generator<SuperComplex.Subject>()
			                   .Include(x => x.Items)
			                   .Pivot()
			                   .Include(x => x.Others)
			                   .ThenInclude(x => x.Another)
			                   .Pivot()
			                   .Include(x => x.YetAnother, include => include.Another2)
			                   .ThenInclude(x => x.Items, x => x.YetAnother2)
			                   .Get();
			subject.Items.Should().HaveCount(3);
			subject.Items.First().Others.Should().HaveCount(3);
			var another = subject.Items.First().Others.First().Another.YetAnother;
			another.Another2.Should().NotBeNull();
			another.Another2.YetAnother.Should().BeSameAs(another);
			another.Another1.Should().BeNull();
			another.Items.Should().HaveCount(3);

			another.Items.First().YetAnother2.Should().BeSameAs(another);
		}

		static class Basic
		{
			public sealed class Subject
			{
				public ICollection<Item> Items { get; set; } = default!;
			}

			public sealed class Item
			{
				public string Name { get; set; } = default!;

				public Subject Subject { get; set; } = default!;
			}
		}

		static class Complex
		{
			public sealed class Subject
			{
				public ICollection<Item> Items { get; set; } = default!;
			}

			public sealed class Item
			{
				[UsedImplicitly]
				public string Name { get; set; } = default!;

				[UsedImplicitly]
				public Subject Subject { get; set; } = default!;

				public ICollection<Other> Others { get; set; } = default!;
			}

			public sealed class Other
			{
				public Subject Subject { get; set; } = default!;

				[UsedImplicitly]
				public Item Item { get; set; } = default!;

				[UsedImplicitly]
				public Another Another { get; set; } = default!;
			}

			public sealed class Another
			{
				[UsedImplicitly]
				public Other Other { get; set; } = default!;
			}
		}

		static class SuperComplex
		{
			public sealed class Subject
			{
				public ICollection<Item> Items { get; set; } = default!;
			}

			public sealed class Item
			{
				[UsedImplicitly]
				public string Name { get; set; } = default!;

				[UsedImplicitly]
				public Subject Subject { get; set; } = default!;

				public ICollection<Other> Others { get; set; } = default!;
			}

			public sealed class Other
			{
				[UsedImplicitly]
				public Subject Subject { get; set; } = default!;

				[UsedImplicitly]
				public Item Item { get; set; } = default!;

				[UsedImplicitly]
				public Another Another { get; set; } = default!;
			}

			public sealed class Another
			{
				[UsedImplicitly]
				public Other Other { get; set; } = default!;

				public YetAnother YetAnother { get; set; } = default!;
			}

			public sealed class YetAnother
			{
				[UsedImplicitly]
				public Another Another1 { get; set; } = default!;

				[UsedImplicitly]
				public Another Another2 { get; set; } = default!;

				public ICollection<SomeItem> Items { get; set; } = default!;
			}

			public sealed class SomeItem
			{
				[UsedImplicitly]
				public YetAnother YetAnother1 { get; set; } = default!;

				public YetAnother YetAnother2 { get; set; } = default!;
			}
		}
	}
}