using DragonSpark.Application.Entities.Queries;
using DragonSpark.Compose;
using DragonSpark.Runtime.Execution;
using DragonSpark.Testing.Objects.Entities;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DragonSpark.Application.Testing.Entities.Queries
{
	public sealed class InputQueryTests
	{
		[Fact]
		public async Task VerifySelected()
		{
			var counter = new Counter();
			var factory = new CounterAwareDbContexts<Context>(new InMemoryDbContexts<Context>(), counter);
			{
				await using var context = factory.CreateDbContext();
				context.Subjects.AddRange(new Subject { Name = "One" }, new Subject { Name = "Two" },
				                          new Subject { Name = "Three" });
				await context.SaveChangesAsync();
			}

			counter.Get().Should().Be(1);

			var evaluate = new EvaluateToArray<Context, string, string>(new DbContexts<Context>(factory), Selected.Default);
			{
				var results = await evaluate.Await("One");
				var open = results.Open();
				open.Should().HaveCount(2);
				open.Should().BeEquivalentTo("Two", "Three");
			}

			counter.Get().Should().Be(2);
		}

		[Fact]
		public async Task VerifySelectedSql()
		{
			await using var factory = await new SqlContexts<Context>().Initialize();
			{
				await using var context = factory.Get();
				context.Subjects.AddRange(new Subject { Name = "One" }, new Subject { Name = "Two" },
				                          new Subject { Name = "Three" });
				await context.SaveChangesAsync();
			}

			var evaluation = new EvaluateToArray<Context, string, string>(factory, Selected.Default);
			{
				var results = await evaluation.Await("One");
				var open = results.Open();
				open.Should().HaveCount(2);
				open.Should().BeEquivalentTo("Two", "Three");
			}

			{
				var results = await evaluation.Await("Two");
				var open    = results.Open();
				open.Should().HaveCount(2);
				open.Should().BeEquivalentTo("One", "Three");
			}
		}

		sealed class Context : DbContext
		{
			public Context(DbContextOptions options) : base(options) {}

			public DbSet<Subject> Subjects { get; set; } = default!;
		}

		sealed class Subject
		{
			[UsedImplicitly]
			public Guid Id { get; set; }

			public string Name { get; set; } = default!;
		}

		sealed class Selected : InputQuery<string, Subject, string>
		{
			public static Selected Default { get; } = new Selected();

			Selected() : base((s, queryable) => queryable.Where(y => y.Name != s).Select(y => y.Name)) {}
		}

	}
}