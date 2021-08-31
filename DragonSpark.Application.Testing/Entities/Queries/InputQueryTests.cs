using BenchmarkDotNet.Attributes;
using DragonSpark.Application.Entities;
using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Application.Entities.Queries.Evaluation;
using DragonSpark.Application.Entities.Queries.Scoped.Materialize.Specialized;
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
	public sealed class InputQueryTests
	{
		[Fact]
		public async Task VerifySelected()
		{
			var counter = new Counter();
			var factory = new CounterAwareDbContexts<Context>(new InMemoryDbContextFactory<Context>(), counter);
			{
				await using var context = factory.CreateDbContext();
				context.Subjects.AddRange(new Subject { Name = "One" }, new Subject { Name = "Two" },
				                          new Subject { Name = "Three" });
				await context.SaveChangesAsync();
			}

			counter.Get().Should().Be(1);

			var evaluate =
				new EvaluateToArray<string, Context, string>(new Contexts<Context>(factory), Selected.Default);
			{
				var results = await evaluate.Await("One");
				var open    = results.Open();
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

			var evaluation = new EvaluateToArray<string, Context, string>(factory, Selected.Default);
			{
				var results = await evaluation.Await("One");
				var open    = results.Open();
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

		[Fact]
		public async Task VerifyComplexSelected()
		{
			var counter = new Counter();
			var contexts =
				new CounterAwareDbContexts<ContextWithData>(new InMemoryDbContextFactory<ContextWithData>(), counter);
			{
				await using var context = contexts.CreateDbContext();
				await context.Database.EnsureCreatedAsync();
			}

			counter.Get().Should().Be(1);

			var id = new Guid("C750443A-19D0-4FD0-B45A-9D1722AD0DB3");

			var evaluate =
				new EvaluateToArray<Input, ContextWithData, string>(new Contexts<ContextWithData>(contexts),
				                                                    ComplexSelected.Default);
			{
				var results = await evaluate.Await(new(id, "One"));
				var only    = results.Open().Only();
				only.Should().NotBeNull();
				only.Should().Be("One");
			}

			counter.Get().Should().Be(2);
		}

		[Fact]
		public async Task VerifyComplexSelectedSql()
		{
			await using var contexts = await new SqlContexts<ContextWithData>().Initialize();
			{
				await using var context = contexts.Get();
				await context.Database.EnsureCreatedAsync();
			}

			var id = new Guid("C750443A-19D0-4FD0-B45A-9D1722AD0DB3");

			var evaluate = contexts.Then().Use(ComplexSelected.Default).To.Array();
			{
				var results = await evaluate.Await(new(id, "One"));
				var only    = results.Open().Only();
				only.Should().NotBeNull();
				only.Should().Be("One");
			}
		}

		[Fact]
		public async Task VerifyWhereWithParameter()
		{
			var contexts = new MemoryContexts<ContextWithData>();
			{
				await using var data = contexts.Get();
				await data.Database.EnsureCreatedAsync();
			}

			var id = new Guid("08013B99-3297-49F6-805E-0A94AE5B79A2");

			var sut = contexts.Then()
			                  .Accept<Input>()
			                  .Use<Subject>()
			                  .Where((input, subject) => subject.Name.StartsWith(input.Name))
			                  .Where((input, subject) => input.Identity == subject.Id)
			                  .To.Single();
			{
				await using var data = contexts.Get();
				var             item = await sut.Await(new Input(id, "Two"));
				item.Should().NotBeNull();
				item.Id.Should().Be(id);
				item.Name.Should().Be("Two");
			}
		}

		[Fact]
		public async Task VerifyWhereWithParameterSql()
		{
			await using var contexts = await new SqlContexts<ContextWithData>().Initialize();
			{
				await using var data = contexts.Get();
				await data.Database.EnsureCreatedAsync();
			}

			var id = new Guid("08013B99-3297-49F6-805E-0A94AE5B79A2");

			var sut = contexts.Then()
			                  .Accept<Input>()
			                  .Use<Subject>()
			                  .Where((input, subject) => subject.Name.StartsWith(input.Name))
			                  .Where((input, subject) => input.Identity == subject.Id)
			                  .Select(x => new Result(x.Id, x.Name))
			                  .To.Single();
			{
				var item = await sut.Await(new Input(id, "Two"));
				item.Identity.Should().Be(id);
				item.Name.Should().Be("Two");
			}
		}

		sealed class Context : DbContext
		{
			public Context(DbContextOptions options) : base(options) {}

			[UsedImplicitly]
			public DbSet<Subject> Subjects { get; set; } = default!;
		}

		sealed class ContextWithData : DbContext
		{
			public ContextWithData(DbContextOptions options) : base(options) {}

			[UsedImplicitly]
			public DbSet<Subject> Subjects { get; set; } = default!;

			protected override void OnModelCreating(ModelBuilder modelBuilder)
			{
				base.OnModelCreating(modelBuilder);
				modelBuilder.Entity<Subject>()
				            .HasData(new Subject { Id = new Guid("C750443A-19D0-4FD0-B45A-9D1722AD0DB3"), Name = "One" },
				                     new Subject
				                     {
					                     Id = new Guid("08013B99-3297-49F6-805E-0A94AE5B79A2"), Name = "Two"
				                     },
				                     new Subject
				                     {
					                     Id = new Guid("5559B8B9-1F19-4F91-A6C8-DE3BB5E47603"), Name = "Three"
				                     });
			}
		}

		sealed class Subject
		{
			[UsedImplicitly]
			public Guid Id { get; set; }

			public string Name { [UsedImplicitly] get; set; } = default!;
		}

		sealed class Selected : StartInput<string, Subject, string>
		{
			public static Selected Default { get; } = new Selected();

			Selected() : base((s, queryable) => queryable.Where(y => y.Name != s).Select(y => y.Name)) {}
		}

		readonly record struct Input(Guid Identity, string Name);

		public readonly record struct Result(Guid Identity, string Name);

		sealed class ComplexSelected : StartInput<Input, Subject, string>
		{
			public static ComplexSelected Default { get; } = new();

			ComplexSelected()
				: base((input, queryable)
					       => queryable.Where(y => y.Id == input.Identity && y.Name == input.Name)
					                   .Select(y => y.Name)) {}
		}

		public class Benchmarks
		{
			readonly IContexts<ContextWithData> _contexts;
			readonly ISelecting<Input, Result>  _select, _scoped;

			public Benchmarks() : this(new DbContextOptionsBuilder<ContextWithData>().UseInMemoryDatabase("0")
			                                                                         .Options) {}

			Benchmarks(DbContextOptions<ContextWithData> options) :
				this(new PooledDbContextFactory<ContextWithData>(options)) {}

			Benchmarks(IDbContextFactory<ContextWithData> factory)
				: this(factory, new Contexts<ContextWithData>(factory)) {}

			Benchmarks(IDbContextFactory<ContextWithData> factory, IContexts<ContextWithData> contexts)
				: this(contexts,
				       contexts.Then()
				               .Accept<Input>()
				               .Use<Subject>()
				               .Where((input, subject)
					                      => subject.Name.StartsWith(input.Name))
				               .Where((input, subject) => input.Identity == subject.Id)
				               .Select(x => new Result(x.Id, x.Name))
				               .To.Single(),
				       new Scoped(factory.CreateDbContext())) {}

			Benchmarks(IContexts<ContextWithData> contexts, ISelecting<Input, Result> select,
			           ISelecting<Input, Result> scoped)
			{
				_contexts = contexts;
				_select   = @select;
				_scoped   = scoped;
			}

			[GlobalSetup]
			public async Task GlobalSetup()
			{
				await using var dbContext = _contexts.Get();
				await dbContext.Database.EnsureCreatedAsync();
			}

			[Benchmark(Baseline = true)]
			public ValueTask<Result> MeasureScoped()
				=> _scoped.Get(new Input(new Guid("08013B99-3297-49F6-805E-0A94AE5B79A2"), "Tw"));

			[Benchmark]
			public ValueTask<Result> MeasureCompiled()
				=> _select.Get(new Input(new Guid("08013B99-3297-49F6-805E-0A94AE5B79A2"), "Tw"));
		}

		sealed class Scoped : SingleSelected<Input, Subject, Result>
		{
			public Scoped(DbContext instance)
				: base(instance.Set<Subject>(),
				       parameter => subject => subject.Id == parameter.Identity &&
				                               subject.Name.StartsWith(parameter.Name), x => new(x.Id, x.Name)) {}
		}
	}
}