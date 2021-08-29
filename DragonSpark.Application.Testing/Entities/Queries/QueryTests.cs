using BenchmarkDotNet.Attributes;
using DragonSpark.Application.Entities.Queries;
using DragonSpark.Application.Entities.Queries.Model;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime.Execution;
using DragonSpark.Testing.Objects.Entities;
using FluentAssertions;
using JetBrains.Annotations;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NetFabric.Hyperlinq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;
using Array = System.Array;

namespace DragonSpark.Application.Testing.Entities.Queries
{
	public sealed class QueryTests
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
				var results  = await evaluate.Await();
				var open = results.Open();
				open.Should().HaveCount(2);
				open.Select(x => x.Name).Should().BeEquivalentTo("One", "Three");
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
				var results  = await evaluate.Await();
				var open = results.Open();
				open.Should().HaveCount(2);
				open.Select(x => x.Name).Should().BeEquivalentTo("One", "Three");
			}

			counter.Get().Should().Be(2);
		}

		[Fact]
		public async Task VerifyParameter()
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

			var evaluate = new SubjectsNotWithParameter(new DbContexts<Context>(factory));
			{
				var results  = await evaluate.Await("One");
				var open = results.Open();
				open.Should().HaveCount(2);
				open.Select(x => x.Name).Should().BeEquivalentTo("Two", "Three");
			}

			counter.Get().Should().Be(2);

			{
				var results  = await evaluate.Await("Two");
				var open = results.Open();
				open.Should().HaveCount(2);
				open.Select(x => x.Name).Should().BeEquivalentTo("One", "Three");
			}

			counter.Get().Should().Be(3);
		}

		[Fact]
		public async Task VerifyParameterSql()
		{
			await using var factory = await new SqlContexts<Context>().Initialize();
			{
				await using var context = factory.Get();
				context.Subjects.AddRange(new Subject { Name = "One" }, new Subject { Name = "Two" },
				                          new Subject { Name = "Three" });
				await context.SaveChangesAsync();
			}

			var parameter = new SubjectsNotWithParameter(factory);
			{
				var results  = await parameter.Await("One");
				var open = results.Open();
				open.Should().HaveCount(2);
				open.Select(x => x.Name).Should().BeEquivalentTo("Two", "Three");
			}

			{
				var results  = await parameter.Await("Two");
				var open = results.Open();
				open.Should().HaveCount(2);
				open.Select(x => x.Name).Should().BeEquivalentTo("One", "Three");
			}
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
				results.Open().Should().BeEquivalentTo(elements);
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

		sealed class Selection : Projector<Subject, Subject>
		{
			public static Selection Default { get; } = new Selection();

			Selection() : base(q => q.Where(x => x.Name != "Two")) {}
		}

		sealed class Parameter : StartInputQuery<string, Subject>
		{
			public static Parameter Default { get; } = new Parameter();

			Parameter() : base((@in, set) => set.Where(x => x.Name != @in)) {}
		}

		sealed class Complex : Start<Subject>
		{
			public static Complex Default { get; } = new Complex();

			Complex() : this(Selection.Default) {}

			public Complex(Expression<Func<IQueryable<Subject>, IQueryable<Subject>>> @select)
				: base((_, subjects) => select.Invoke(subjects)) {}
		}

		sealed class SubjectsNotTwo : EvaluateToArray<Context, Subject>
		{
			public SubjectsNotTwo(IContexts<Context> contexts) : base(contexts, Query.Default) {}

			public SubjectsNotTwo(IContexts<Context> contexts, IQuery<Subject> query) : base(contexts, query) {}
		}

		sealed class SubjectsNotWithParameter : EvaluateToArray<Context, string, Subject>
		{
			public SubjectsNotWithParameter(IContexts<Context> contexts) : base(contexts, Parameter.Default) {}
		}

		public class Benchmarks
		{
			readonly IResulting<Array<Subject>>         _query;
			readonly ISelecting<string, Array<Subject>> _selected;
			readonly IQueryable<Subject>                _scoped;

			public Benchmarks() : this(new DbContextOptionsBuilder<Context>().UseInMemoryDatabase("0").Options) {}

			Benchmarks(DbContextOptions<Context> options) : this(new PooledDbContextFactory<Context>(options)) {}

			Benchmarks(IDbContextFactory<Context> factory)
				: this(new SubjectsNotTwo(new DbContexts<Context>(factory)),
				       new SubjectsNotWithParameter(new DbContexts<Context>(factory)),
				       new Scoped(factory.CreateDbContext()).Get()) {}

			Benchmarks(IResulting<Array<Subject>> query, ISelecting<string, Array<Subject>> selected,
			           IQueryable<Subject> scoped)
			{
				_query    = query;
				_selected = selected;
				_scoped   = scoped;
			}

			[Benchmark(Baseline = true)]
			public async Task<Array> MeasureScoped() => await _scoped.ToArrayAsync();

			[Benchmark]
			public async Task<Array> MeasureCompiled() => await _query.Await();

			[Benchmark]
			public async Task<Array> MeasureCompiledParameter() => await _selected.Await("Two");
		}

		/*
		public class Benchmarks
		{
			readonly DbContext                      _subject;
			readonly IAssignable<DbContext, string> _select;

			public Benchmarks() : this(new DbContextOptionsBuilder<Context>().UseInMemoryDatabase("0").Options) {}

			Benchmarks(DbContextOptions<Context> options) : this(new PooledDbContextFactory<Context>(options)) {}

			Benchmarks(IDbContextFactory<Context> factory)
				: this(factory.CreateDbContext(), Parameters<string>.Default) {}

			Benchmarks(DbContext subject, IAssignable<DbContext, string> select)
			{
				_subject = subject;
				_select  = @select;
			}

			[Benchmark(Baseline = true)]
			public string MeasureAccess() => _select.Get(_subject);

			[Benchmark]
			public None MeasureAssign()
			{
				_select.Assign(_subject, "None.Default");
				return None.Default;
			}

			[Benchmark]
			public string MeasureFull()
			{
				_select.Assign(_subject, "None.Default");
				return _select.Get(_subject);
			}
		}
	*/
	}
}