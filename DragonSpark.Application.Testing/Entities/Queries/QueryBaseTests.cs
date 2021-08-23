using BenchmarkDotNet.Attributes;
using DragonSpark.Application.Entities.Queries;
using DragonSpark.Application.Entities.Queries.Scoped;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Runtime.Execution;
using DragonSpark.Testing.Objects.Entities;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DragonSpark.Application.Testing.Entities.Queries
{
	public sealed class QueryBaseTests
	{
		[Fact]
		public async Task VerifyInMemorySubjects()
		{
			var factory = new InMemoryDbContexts<Context>();
			{
				await using var context = factory.CreateDbContext();
				context.Subjects.AddRange(new Subject { Name = "One" }, new Subject { Name = "Two" },
				                          new Subject { Name = "Three" });
				await context.SaveChangesAsync();
			}

			{
				await using var context = factory.CreateDbContext();
				var             only    = await context.Subjects.SingleOrDefaultAsync(x => x.Name == "One");
				only.Should().NotBeNull();
			}
		}

		[Fact]
		public async Task VerifyQuery()
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

			var evaluate = new SubjectsNotTwo(new DbContexts<Context>(factory));
			{
				var results = await evaluate.Await();
				results.Should().HaveCount(2);
				results.Select(x => x.Name).Should().BeEquivalentTo("One", "Three");
			}

			counter.Get().Should().Be(2);
		}

		[Fact]
		public async Task VerifyEvaluate()
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

			var evaluate = new SubjectsNotTwo(new DbContexts<Context>(factory));
			{
				var results = await evaluate.Await();
				results.Should().HaveCount(2);
				results.Select(x => x.Name).Should().BeEquivalentTo("One", "Three");
			}

			counter.Get().Should().Be(2);
		}

		[Fact]
		public async Task VerifyEquality()
		{
			var factory = new InMemoryDbContexts<Context>();
			{
				await using var context = factory.CreateDbContext();
				context.Subjects.AddRange(new Subject { Name = "One" }, new Subject { Name = "Two" },
				                          new Subject { Name = "Three" });
				await context.SaveChangesAsync();
			}

			var evaluate = new SubjectsNotTwo(new DbContexts<Context>(factory));
			{
				var             results  = await evaluate.Await();
				await using var context  = factory.CreateDbContext();
				var             scoped   = new Scoped(context);
				var             elements = await scoped.Get().ToArrayAsync();
				results.Should().BeEquivalentTo(elements);
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

		sealed class Scoped : Query<Subject>
		{
			public Scoped(Context instance) : base(instance.Set<Subject>().Where(x => x.Name != "Two")) {}
		}

		sealed class Query : Append<Subject>
		{
			public static Query Default { get; } = new Query();

			Query() : base(q => q.Where(x => x.Name != "Two")) {}
		}

		sealed class SubjectsNotTwo : EvaluateToArray<Context, Subject>
		{
			public SubjectsNotTwo(IContexts<Context> contexts) : base(contexts, Query.Default) {}
		}

		public class Benchmarks
		{
			readonly IResulting<Subject[]> _query;
			readonly IQueryable<Subject>   _scoped;

			public Benchmarks() : this(new DbContextOptionsBuilder<Context>().UseInMemoryDatabase("0").Options) {}

			Benchmarks(DbContextOptions<Context> options) : this(new PooledDbContextFactory<Context>(options)) {}

			Benchmarks(IDbContextFactory<Context> factory)
				: this(new SubjectsNotTwo(new DbContexts<Context>(factory)),
				       new Scoped(factory.CreateDbContext()).Get()) {}

			Benchmarks(IResulting<Subject[]> query, IQueryable<Subject> scoped)
			{
				_query  = query;
				_scoped = scoped;
			}

			[Benchmark]
			public async Task<Array> MeasureSingleton() => await _query.Await();

			[Benchmark(Baseline = true)]
			public async Task<Array> MeasureScoped() => await _scoped.ToArrayAsync();
		}
	}
}