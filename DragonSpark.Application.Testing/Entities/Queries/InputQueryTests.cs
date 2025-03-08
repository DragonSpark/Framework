using BenchmarkDotNet.Attributes;
using DragonSpark.Application.AspNet;
using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;
using DragonSpark.Application.AspNet.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Runtime.Execution;
using DragonSpark.Testing.Objects.Entities;
using DragonSpark.Testing.Objects.Entities.SqlLite;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DragonSpark.Application.Testing.Entities.Queries;

public sealed class InputQueryTests
{
	[Fact]
	public async Task VerifySelected()
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

		var evaluate =
			new EvaluateToArray<string, string>(new NewContext<Context>(factory).Then().Scopes(),
			                                    Selected.Default);
		{
			var results = await evaluate.Off("One");
			var open    = results.Open();
			open.Should().HaveCount(2);
			open.Should().BeEquivalentTo("Two", "Three");
		}

		counter.Get().Should().Be(2);
	}

	[Fact]
	public async Task VerifyExternalParameter()
	{
		var contexts = new MemoryNewContext<Context>();
		{
			await using var context = contexts.Get();
			context.Subjects.AddRange(new Subject { Name = "One" }, new Subject { Name = "Two" },
			                          new Subject { Name = "Three" });
			await context.SaveChangesAsync();
		}

		const string name = "Two";

		var sut = Start.A.Query<Subject>()
		               .Accept<string>()
		               .Where((s, subject) => subject.Name == name || subject.Name == s)
		               .Invoke(contexts)
		               .To.Array();
		var subjects = await sut.Off("One");
		var open     = subjects.Open();
		open.Should().HaveCount(2);
		open.Select(x => x.Name).Should().BeEquivalentTo("Two", "One");
	}

	[Fact]
	public async Task VerifyExternalParameterSql()
	{
		await using var contexts = await new SqlLiteNewContext<Context>().Initialize();
		
		{
			await using var context = contexts.Get();
			context.Subjects.AddRange(new Subject { Name = "One" }, new Subject { Name = "Two" },
			                          new Subject { Name = "Three" });
			await context.SaveChangesAsync();
		}

		const string name = "Two";

		var sut = Start.A.Query<Subject>()
		               .Accept<string>()
		               .Where((s, subject) => subject.Name == name || subject.Name == s)
		               .Invoke(contexts)
		               .To.Array();
		var subjects = await sut.Off("One");
		var open     = subjects.Open();
		open.Should().HaveCount(2);
		open.Select(x => x.Name).Should().BeEquivalentTo("Two", "One");
	}

	[Fact]
	public async Task VerifySelectedSql()
	{
		await using var factory = await new SqlLiteNewContext<Context>().Initialize();
		
		{
			await using var context = factory.Get();
			context.Subjects.AddRange(new Subject { Name = "One" }, new Subject { Name = "Two" },
			                          new Subject { Name = "Three" });
			await context.SaveChangesAsync();
		}

		var evaluation = new EvaluateToArray<string, string>(factory.Then().Scopes(), Selected.Default);
		{
			var results = await evaluation.Off("One");
			var open    = results.Open();
			open.Should().HaveCount(2);
			open.Should().BeEquivalentTo("Two", "Three");
		}

		{
			var results = await evaluation.Off("Two");
			var open    = results.Open();
			open.Should().HaveCount(2);
			open.Should().BeEquivalentTo("One", "Three");
		}
	}

	[Fact]
	public async Task VerifyComplexSelected()
	{
		var counter = new SafeCounter();
		var contexts =
			new CounterAwareDbContexts<ContextWithData>(new InMemoryDbContextFactory<ContextWithData>(), counter);
		{
			await using var context = contexts.CreateDbContext();
			await context.Database.EnsureCreatedAsync();
		}

		counter.Get().Should().Be(1);

		var id = new Guid("C750443A-19D0-4FD0-B45A-9D1722AD0DB3");

		var evaluate =
			new EvaluateToArray<Input, string>(new NewContext<ContextWithData>(contexts).Then().Scopes(),
			                                   ComplexSelected.Default);
		{
			var results = await evaluate.Off(new(id, "One"));
			var only    = results.Open().Only();
			only.Should().NotBeNull();
			only.Should().Be("One");
		}

		counter.Get().Should().Be(2);
	}

	[Fact]
	public async Task VerifyComplexSelectedSql()
	{
		await using var contexts = await new SqlLiteNewContext<ContextWithData>().Initialize();
		
		{
			await using var context = contexts.Get();
			await context.Database.EnsureCreatedAsync();
		}

		var id = new Guid("C750443A-19D0-4FD0-B45A-9D1722AD0DB3");

		var evaluate = contexts.Then().Use(ComplexSelected.Default).To.Array();
		{
			var results = await evaluate.Off(new(id, "One"));
			var only    = results.Open().Only();
			only.Should().NotBeNull();
			only.Should().Be("One");
		}
	}

	[Fact]
	public async Task VerifyWhereWithParameter()
	{
		var contexts = new MemoryNewContext<ContextWithData>();
		{
			await using var data = contexts.Get();
			await data.Database.EnsureCreatedAsync();
		}

		var id = new Guid("08013B99-3297-49F6-805E-0A94AE5B79A2");

		var sut = Start.A.Query<Subject>()
		               .Accept<Input>()
		               .Where((input, subject) => subject.Name.StartsWith(input.Name))
		               .Where((input, subject) => input.Identity == subject.Id)
		               .Invoke(contexts)
		               .To.Single();
		{
			await using var data = contexts.Get();
			var             item = await sut.Off(new Input(id, "Two"));
			item.Should().NotBeNull();
			item.Id.Should().Be(id);
			item.Name.Should().Be("Two");
		}
	}

	[Fact]
	public async Task VerifyWhereWithParameterSql()
	{
		await using var contexts = await new SqlLiteNewContext<ContextWithData>().Initialize();
		
		{
			await using var data = contexts.Get();
			await data.Database.EnsureCreatedAsync();
		}

		var id = new Guid("08013B99-3297-49F6-805E-0A94AE5B79A2");

		var sut = Start.A.Query<Subject>()
		               .Accept<Input>()
		               .Where((input, subject) => subject.Name.StartsWith(input.Name))
		               .Where((input, subject) => input.Identity == subject.Id)
		               .Select(x => new Result(x.Id, x.Name))
		               .Invoke(contexts)
		               .To.Single();
		{
			var item = await sut.Off(new Input(id, "Two"));
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

		[MaxLength(64)]
		public string Name { [UsedImplicitly] get; set; } = default!;
	}

	sealed class Selected : StartInput<string, Subject, string>
	{
		public static Selected Default { get; } = new();

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
		readonly INewContext<ContextWithData> _new;
		readonly ISelecting<Input, Result>  _select;

		public Benchmarks() : this(new DbContextOptionsBuilder<ContextWithData>().UseInMemoryDatabase("0")
		                                                                         .Options) {}

		Benchmarks(DbContextOptions<ContextWithData> options) :
			this(new PooledDbContextFactory<ContextWithData>(options)) {}

		Benchmarks(IDbContextFactory<ContextWithData> factory) : this(new NewContext<ContextWithData>(factory)) {}

		Benchmarks(INewContext<ContextWithData> @new)
			: this(@new,
			       Start.A.Query<Subject>()
			            .Accept<Input>()
			            .Where((input, subject)
				                   => subject.Name.StartsWith(input.Name))
			            .Where((input, subject) => input.Identity == subject.Id)
			            .Select(x => new Result(x.Id, x.Name))
			            .Invoke(@new)
			            .To.Single()) {}

		Benchmarks(INewContext<ContextWithData> @new, ISelecting<Input, Result> select)
		{
			_new = @new;
			_select   = @select;
		}

		[GlobalSetup]
		public async Task GlobalSetup()
		{
			await using var dbContext = _new.Get();
			await dbContext.Database.EnsureCreatedAsync();
		}

		[Benchmark]
		public ValueTask<Result> MeasureCompiled()
			=> _select.Get(new Input(new Guid("08013B99-3297-49F6-805E-0A94AE5B79A2"), "Tw"));
	}
}