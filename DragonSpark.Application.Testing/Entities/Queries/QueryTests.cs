using BenchmarkDotNet.Attributes;
using DragonSpark.Application.AspNet;
using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Application.AspNet.Entities.Queries.Compiled;
using DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;
using DragonSpark.Application.AspNet.Entities.Queries.Composition;
using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Selection;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime.Execution;
using DragonSpark.Testing.Objects.Entities;
using DragonSpark.Testing.Objects.Entities.SqlLite;
using FluentAssertions;
using JetBrains.Annotations;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NetFabric.Hyperlinq;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;
using Array = System.Array;

namespace DragonSpark.Application.Testing.Entities.Queries;

public sealed class QueryTests
{
	[Fact]
	public async Task VerifyInMemorySubjects()
	{
		var factory = new InMemoryDbContextFactory<Context>();
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
		var counter = new SafeCounter();
		var factory = new CounterAwareDbContexts<Context>(new InMemoryDbContextFactory<Context>(), counter);
		{
			await using var context = factory.CreateDbContext();
			context.Subjects.AddRange(new Subject { Name = "One" }, new Subject { Name = "Two" },
			                          new Subject { Name = "Three" });
			await context.SaveChangesAsync();
		}

		counter.Get().Should().Be(1);

		var contexts = new NewContext<Context>(factory);
		var invoke   = new Reading<None, Subject>(new Scopes<Context>(contexts), Query.Default);
		{
			using var invocation = invoke.Get(None.Default);
			var       elements   = await invocation.Elements.AsAsyncValueEnumerable().ToArrayAsync();
			elements.Should().HaveCount(2);
			elements.Select(x => x.Name).Should().BeEquivalentTo("One", "Three");
		}

		counter.Get().Should().Be(2);
	}

	[Fact]
	public async Task VerifyEvaluate()
	{
		var counter = new SafeCounter();
		var factory = new CounterAwareDbContexts<Context>(new InMemoryDbContextFactory<Context>(), counter);
		{
			await using var context = factory.CreateDbContext();
			context.Subjects.AddRange(new Subject { Name = "One" }, new Subject { Name = "Two" },
			                          new Subject { Name = "Three" });
			await context.SaveChangesAsync();
		}

		counter.Get().Should().Be(1);

		var evaluate = new SubjectsNotTwo(new NewContext<Context>(factory));
		{
			var results = await evaluate.Off();
			var open    = results.Open();
			open.Should().HaveCount(2);
			open.Select(x => x.Name).Should().BeEquivalentTo("One", "Three");
		}

		counter.Get().Should().Be(2);
	}

	[Fact]
	public async Task VerifyComplex()
	{
		var counter = new SafeCounter();
		var factory = new CounterAwareDbContexts<Context>(new InMemoryDbContextFactory<Context>(), counter);
		{
			await using var context = factory.CreateDbContext();
			context.Subjects.AddRange(new Subject { Name = "One" }, new Subject { Name = "Two" },
			                          new Subject { Name = "Three" });
			await context.SaveChangesAsync();
		}

		counter.Get().Should().Be(1);

		var evaluate = new SubjectsNotTwo(new NewContext<Context>(factory), Complex.Default);
		{
			var results = await evaluate.Off();
			var open    = results.Open();
			open.Should().HaveCount(2);
			open.Select(x => x.Name).Should().BeEquivalentTo("One", "Three");
		}

		counter.Get().Should().Be(2);
	}

	[Fact]
	public async Task VerifyParameter()
	{
		var counter = new SafeCounter();
		var factory = new CounterAwareDbContexts<Context>(new InMemoryDbContextFactory<Context>(), counter);
		{
			await using var context = factory.CreateDbContext();
			context.Subjects.AddRange(new Subject { Name = "One" }, new Subject { Name = "Two" },
			                          new Subject { Name = "Three" });
			await context.SaveChangesAsync();
		}

		counter.Get().Should().Be(1);

		var evaluate = new SubjectsNotWithParameter(new NewContext<Context>(factory));
		{
			var results = await evaluate.Off("One");
			var open    = results.Open();
			open.Should().HaveCount(2);
			open.Select(x => x.Name).Should().BeEquivalentTo("Two", "Three");
		}

		counter.Get().Should().Be(2);

		{
			var results = await evaluate.Off("Two");
			var open    = results.Open();
			open.Should().HaveCount(2);
			open.Select(x => x.Name).Should().BeEquivalentTo("One", "Three");
		}

		counter.Get().Should().Be(3);
	}

	[Fact]
	public async Task VerifyParameterSql()
	{
		await using var factory = await new SqlLiteNewContext<Context>().Initialize();

		{
			await using var context = factory.Get();
			context.Subjects.AddRange(new Subject { Name = "One" }, new Subject { Name = "Two" },
			                          new Subject { Name = "Three" });
			await context.SaveChangesAsync();
		}

		var parameter = new SubjectsNotWithParameter(factory);
		{
			var results = await parameter.Off("One");
			var open    = results.Open();
			open.Should().HaveCount(2);
			open.Select(x => x.Name).Should().BeEquivalentTo("Two", "Three");
		}

		{
			var results = await parameter.Off("Two");
			var open    = results.Open();
			open.Should().HaveCount(2);
			open.Select(x => x.Name).Should().BeEquivalentTo("One", "Three");
		}
	}

	[Fact]
	public async Task VerifyEquality()
	{
		var factory = new InMemoryDbContextFactory<Context>();
		{
			await using var context = factory.CreateDbContext();
			context.Subjects.AddRange(new Subject { Name = "One" }, new Subject { Name = "Two" },
			                          new Subject { Name = "Three" });
			await context.SaveChangesAsync();
		}

		var evaluate = new SubjectsNotTwo(new NewContext<Context>(factory));
		{
			var             results  = await evaluate.Off();
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

		[MaxLength(64)]
		public string Name { get; set; } = default!;
	}

	sealed class Scoped : AspNet.Entities.Queries.Runtime.Selection.Query<Subject>
	{
		public Scoped(Context instance) : base(instance.Set<Subject>().Where(x => x.Name != "Two")) {}
	}

	sealed class Query : Start<Subject>
	{
		public static Query Default { get; } = new();

		Query() : base(q => q.Where(x => x.Name != "Two")) {}
	}

	sealed class Selection : Projector<Subject, Subject>
	{
		public static Selection Default { get; } = new();

		Selection() : base(q => q.Where(x => x.Name != "Two")) {}
	}

	sealed class Parameter : StartInput<string, Subject>
	{
		public static Parameter Default { get; } = new();

		Parameter() : base((@in, set) => set.Where(x => x.Name != @in)) {}
	}

	sealed class Complex : Start<Subject>
	{
		public static Complex Default { get; } = new();

		Complex() : this(Selection.Default) {}

		public Complex(Expression<Func<IQueryable<Subject>, IQueryable<Subject>>> @select)
			: base((_, subjects) => select.Invoke(subjects)) {}
	}

	sealed class SubjectsNotTwo : EvaluateToArray<None, Subject>
	{
		public SubjectsNotTwo(INewContext<Context> @new) : base(@new.Then().Scopes(), Query.Default) {}

		public SubjectsNotTwo(INewContext<Context> @new, IQuery<Subject> query)
			: base(@new.Then().Scopes(), query.Get()) {}
	}

	sealed class SubjectsNotWithParameter : EvaluateToArray<string, Subject>
	{
		public SubjectsNotWithParameter(INewContext<Context> @new)
			: base(@new.Then().Scopes(), Parameter.Default) {}
	}

	public class Benchmarks
	{
		readonly ISelecting<None, Array<Subject>>   _query;
		readonly ISelecting<string, Array<Subject>> _selected;
		readonly IQueryable<Subject>                _scoped;

		public Benchmarks() : this(new DbContextOptionsBuilder<Context>().UseInMemoryDatabase("0").Options) {}

		Benchmarks(DbContextOptions<Context> options) : this(new PooledDbContextFactory<Context>(options)) {}

		Benchmarks(IDbContextFactory<Context> factory)
			: this(new SubjectsNotTwo(new NewContext<Context>(factory)),
			       new SubjectsNotWithParameter(new NewContext<Context>(factory)),
			       new Scoped(factory.CreateDbContext()).Get()) {}

		Benchmarks(ISelecting<None, Array<Subject>> query, ISelecting<string, Array<Subject>> selected,
		           IQueryable<Subject> scoped)
		{
			_query    = query;
			_selected = selected;
			_scoped   = scoped;
		}

		[Benchmark(Baseline = true)]
		public async Task<Array> MeasureScoped() => await _scoped.ToArrayAsync();

		[Benchmark]
		public async Task<Array> MeasureCompiled() => await _query.Off();

		[Benchmark]
		public async Task<Array> MeasureCompiledParameter() => await _selected.Off("Two");
	}
}