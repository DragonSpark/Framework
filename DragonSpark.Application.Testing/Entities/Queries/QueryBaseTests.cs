using BenchmarkDotNet.Attributes;
using DragonSpark.Application.Entities.Queries;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime.Execution;
using DragonSpark.Testing.Objects.Entities;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NetFabric.Hyperlinq;
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
		public async Task VerifyInvoke()
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

			var contexts = new DbContexts<Context>(factory);
			var invoke   = new Invoke<Context, Subject>(contexts, Query.Default);
			{
				await using var invocation = invoke.Get(None.Default);
				var             elements   = await invocation.Elements.AsAsyncValueEnumerable().ToArrayAsync();
				elements.Should().HaveCount(2);
				elements.Select(x => x.Name).Should().BeEquivalentTo("One", "Three");
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
		public async Task VerifyComplex()
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

			var evaluate = new SubjectsNotTwo(new DbContexts<Context>(factory), Complex.Default);
			{
				var results = await evaluate.Await();
				results.Should().HaveCount(2);
				results.Select(x => x.Name).Should().BeEquivalentTo("One", "Three");
			}

			counter.Get().Should().Be(2);
		}

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

			var evaluate = new SubjectsNotTwo(new DbContexts<Context>(factory), Selected.Default);
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

		sealed class Scoped : Application.Entities.Queries.Scoped.Query<Subject>
		{
			public Scoped(Context instance) : base(instance.Set<Subject>().Where(x => x.Name != "Two")) {}
		}

		sealed class Query : Start<Subject>
		{
			public static Query Default { get; } = new Query();

			Query() : base(q => q.Where(x => x.Name != "Two")) {}
		}

		sealed class Selection : Select<IQueryable<Subject>, IQueryable<Subject>>
		{
			public static Selection Default { get; } = new Selection();

			Selection() : base(q => q.Where(x => x.Name != "Two")) {}
		}

		sealed class Selected : Selected<None, Subject>
		{
			public static Selected Default { get; } = new Selected();

			Selected() : base(@in => @in.Context.Set<Subject>().Where(x => x.Name != "Two")) {}
		}

		sealed class Complex : Start<Subject>
		{
			public static Complex Default { get; } = new Complex();

			Complex() : this(Selection.Default.Get) {}

			public Complex(Func<IQueryable<Subject>, IQueryable<Subject>> @select)
				: base((_, subjects) => select(subjects)) {}
		}

		sealed class SubjectsNotTwo : EvaluateToArray<Context, None, Subject>
		{
			public SubjectsNotTwo(IContexts<Context> contexts) : this(contexts, Query.Default) {}

			public SubjectsNotTwo(IContexts<Context> contexts, IQuery<None, Subject> query) : base(contexts, query) {}
		}

		public class Benchmarks
		{
			readonly ISelecting<None, Subject[]> _query;
			readonly IQueryable<Subject>         _scoped;

			public Benchmarks() : this(new DbContextOptionsBuilder<Context>().UseInMemoryDatabase("0").Options) {}

			Benchmarks(DbContextOptions<Context> options) : this(new PooledDbContextFactory<Context>(options)) {}

			Benchmarks(IDbContextFactory<Context> factory)
				: this(new SubjectsNotTwo(new DbContexts<Context>(factory)),
				       new Scoped(factory.CreateDbContext()).Get()) {}

			Benchmarks(ISelecting<None, Subject[]> query, IQueryable<Subject> scoped)
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