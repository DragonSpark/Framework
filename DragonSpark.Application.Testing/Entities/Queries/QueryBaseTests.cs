using BenchmarkDotNet.Attributes;
using DragonSpark.Application.Entities.Queries.Scoped;
using DragonSpark.Application.Entities.Queries.Transactional;
using DragonSpark.Runtime.Execution;
using DragonSpark.Testing.Objects.Entities;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
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
			var source = new DbQuery<Context, Subject>(new DbQueryMaps<Context>(factory));
			var query  = new Query(source);

			var store = new Dictionary<Type, IMap>();
			{
				await using var session = new Session(store);
				var             results = await query.Get(session).ToArrayAsync();
				results.Should().HaveCount(2);
				store.Values.Should().HaveCount(1);
				results.Select(x => x.Name).Should().BeEquivalentTo("One", "Three");
			}
			counter.Get().Should().Be(2);
			store.Values.Should().BeEmpty();
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

		sealed class Query : DbQueryBase<Context, Subject>
		{
			public Query(DbQuery<Context, Subject> previous) : base(previous, q => q.Where(x => x.Name != "Two")) {}
		}

		public class Benchmarks
		{
			readonly IQuery<Subject> _query;
			readonly Query<Subject>  _scoped;

			public Benchmarks()
				: this(new PooledDbContextFactory<Context>(new DbContextOptionsBuilder<Context>()
				                                           .UseInMemoryDatabase("0")
				                                           .Options)) {}

			Benchmarks(IDbContextFactory<Context> factory)
				: this(new Query(new DbQuery<Context, Subject>(new DbQueryMaps<Context>(factory))),
				       new Scoped(factory.CreateDbContext())) {}

			Benchmarks(IQuery<Subject> query, Query<Subject> scoped)
			{
				_query  = query;
				_scoped = scoped;
			}

			[Benchmark]
			public async Task<object> MeasureSingleton()
			{
				await using var session = new Session();
				var             results = await _query.Get(session).ToArrayAsync();
				return results;
			}

			[Benchmark(Baseline = true)]
			public async Task<object> MeasureScoped() => await _scoped.Get().ToArrayAsync();
		}
	}
}