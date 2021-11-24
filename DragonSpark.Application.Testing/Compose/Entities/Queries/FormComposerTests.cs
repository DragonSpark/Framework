using BenchmarkDotNet.Attributes;
using DragonSpark.Application.Entities;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Testing.Objects.Entities;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DragonSpark.Application.Testing.Compose.Entities.Queries;

public sealed class FormComposerTests
{
	[Fact]
	public async Task VerifyArray()
	{
		var contexts = new MemoryContexts<Context>();
		{
			await using var context = contexts.Get();
			await context.Database.EnsureCreatedAsync();
		}
		{
			var result = await Start.A.Query<Subject>()
			                        .Where(x => x.Name != "Two")
			                        .Invoke(contexts)
			                        .To.Array()
			                        .Get();
			result.Length.Should().Be(2);
			result.Open().Select(x => x.Name).Should().BeEquivalentTo("One", "Three");
		}
	}

	[Fact]
	public async Task VerifyLease()
	{
		var contexts = new Contexts<Context>(new InMemoryDbContextFactory<Context>());
		{
			await using var context = contexts.Get();
			await context.Database.EnsureCreatedAsync();
		}
		{
			using var result =
				await Start.A.Query<Subject>().Where(x => x.Name != "Two").Invoke(contexts).To.Lease().Get();
			result.Length.Should().Be(2);
			result.ToArray().Select(x => x.Name).Should().BeEquivalentTo("One", "Three");
		}
	}

	[Fact]
	public async Task VerifyList()
	{
		var contexts = new Contexts<Context>(new InMemoryDbContextFactory<Context>());
		{
			await using var context = contexts.Get();
			await context.Database.EnsureCreatedAsync();
		}
		{
			var result = await Start.A.Query<Subject>()
			                        .Where(x => x.Name != "Two")
			                        .Invoke(contexts)
			                        .To.List()
			                        .Get();
			result.Count.Should().Be(2);
			result.Select(x => x.Name).Should().BeEquivalentTo("One", "Three");
		}
	}

	[Fact]
	public async Task VerifyDictionary()
	{
		var contexts = new Contexts<Context>(new InMemoryDbContextFactory<Context>());
		{
			await using var context = contexts.Get();
			await context.Database.EnsureCreatedAsync();
		}
		{
			var result = await Start.A.Query<Subject>()
			                        .Where(x => x.Name != "Two")
			                        .Invoke(contexts)
			                        .To.Dictionary(x => x.Name)
			                        .Get();
			result.Count.Should().Be(2);
			result.Keys.Should().BeEquivalentTo("One", "Three");
		}
	}

	[Fact]
	public async Task VerifyDictionaryValues()
	{
		var contexts = new Contexts<Context>(new InMemoryDbContextFactory<Context>());
		{
			await using var context = contexts.Get();
			await context.Database.EnsureCreatedAsync();
		}
		{
			var result = await Start.A.Query<Subject>()
			                        .Where(x => x.Name != "Two")
			                        .Invoke(contexts)
			                        .To.Dictionary(x => x.Name, x => x.Id)
			                        .Get();
			result.Count.Should().Be(2);
			result.Keys.Count.Should().Be(2);
			result.Keys.Should().BeEquivalentTo("One", "Three");
			result.Values.Count.Should().Be(2);
			foreach (var value in result.Values)
			{
				value.Should().NotBeEmpty();
			}
		}
	}

	[Fact]
	public async Task VerifySingle()
	{
		var contexts = new Contexts<Context>(new InMemoryDbContextFactory<Context>());
		{
			await using var context = contexts.Get();
			await context.Database.EnsureCreatedAsync();
		}
		{
			var single = await Start.A.Query<Subject>()
			                        .Where(x => x.Name == "Two")
			                        .Invoke(contexts)
			                        .To.Single()
			                        .Get();
			single.Should().NotBeNull();
		}
		{
			var single = Start.A.Query<Subject>().Where(x => x.Name != "Two").Invoke(contexts).To.Single().Get();
			await single.Awaiting(x => x.AsTask()).Should().ThrowAsync<InvalidOperationException>();
		}
	}

	[Fact]
	public async Task VerifySingleOrDefault()
	{
		var contexts = new Contexts<Context>(new InMemoryDbContextFactory<Context>());
		{
			await using var context = contexts.Get();
			await context.Database.EnsureCreatedAsync();
		}
		{
			var single = await Start.A.Query<Subject>()
			                        .Where(x => x.Name == "Two")
			                        .Invoke(contexts)
			                        .To.SingleOrDefault()
			                        .Get();
			single.Should().NotBeNull();
		}
		{
			var single = await Start.A.Query<Subject>()
			                        .Where(x => x.Name == "Four")
			                        .Invoke(contexts)
			                        .To.SingleOrDefault()
			                        .Get();
			single.Should().BeNull();
		}
	}

	[Fact]
	public async Task VerifyFirst()
	{
		var contexts = new MemoryContexts<Context>();
		{
			await using var context = contexts.Get();
			await context.Database.EnsureCreatedAsync();
		}
		{
			var single = await Start.A.Query<Subject>()
			                        .Where(x => x.Name == "Two")
			                        .Invoke(contexts)
			                        .To.First()
			                        .Get();
			single.Should().NotBeNull();
		}
		{
			var single = Start.A.Query<Subject>().Where(x => x.Name == "Four").Invoke(contexts).To.First().Get();
			await single.Awaiting(x => x.AsTask()).Should().ThrowAsync<InvalidOperationException>();
		}
	}

	[Fact]
	public async Task VerifyFirstOrDefault()
	{
		var contexts = new Contexts<Context>(new InMemoryDbContextFactory<Context>());
		{
			await using var context = contexts.Get();
			await context.Database.EnsureCreatedAsync();
		}
		{
			var single = await Start.A.Query<Subject>()
			                        .Where(x => x.Name == "Two")
			                        .Invoke(contexts)
			                        .To.FirstOrDefault()
			                        .Get();
			single.Should().NotBeNull();
		}
		{
			var single = await Start.A.Query<Subject>()
			                        .Where(x => x.Name == "Four")
			                        .Invoke(contexts)
			                        .To.FirstOrDefault()
			                        .Get();
			single.Should().BeNull();
		}
	}

	[Fact]
	public async Task VerifyAny()
	{
		var contexts = new Contexts<Context>(new InMemoryDbContextFactory<Context>());
		{
			await using var context = contexts.Get();
			await context.Database.EnsureCreatedAsync();
		}
		{
			var single = await Start.A.Query<Subject>().Where(x => x.Name == "Two").Invoke(contexts).To.Any().Get();
			single.Should().BeTrue();
		}
		{
			var single = await Start.A.Query<Subject>()
			                        .Where(x => x.Name == "Four")
			                        .Invoke(contexts)
			                        .To.Any()
			                        .Get();
			single.Should().BeFalse();
		}
	}

	sealed class Context : DbContext
	{
		public Context(DbContextOptions options) : base(options) {}

		[UsedImplicitly]
		public DbSet<Subject> Subjects { get; set; } = default!;

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Subject>()
			            .HasData(new Subject { Id = Guid.NewGuid(), Name = "One" },
			                     new Subject { Id = Guid.NewGuid(), Name = "Two" },
			                     new Subject { Id = Guid.NewGuid(), Name = "Three" });
		}
	}

	public sealed class Subject
	{
		[UsedImplicitly]
		public Guid Id { get; set; }

		public string Name { get; set; } = default!;
	}

	public class Benchmarks
	{
		readonly ISelecting<None, Subject>      _single;
		readonly IDbContextFactory<Context>     _factory;
		readonly Func<DbContext, Task<Subject>> _create;

		public Benchmarks() : this(new InMemoryDbContextFactory<Context>()) {}

		Benchmarks(IDbContextFactory<Context> factory) : this(new Contexts<Context>(factory), factory) {}

		Benchmarks(IContexts<Context> contexts, IDbContextFactory<Context> factory)
			: this(Start.A.Query<Subject>().Where(x => x.Name == "Two").Invoke(contexts).To.Single(),
			       factory,
			       EF.CompileAsyncQuery<DbContext, Subject>(x => x.Set<Subject>().Single(y => y.Name == "Two"))) {}

		Benchmarks(ISelecting<None, Subject> single, IDbContextFactory<Context> factory,
		           Func<DbContext, Task<Subject>> create)
		{
			_single  = single;
			_factory = factory;
			_create  = create;
		}

		[GlobalSetup]
		public async Task GlobalSetup()
		{
			await using var dbContext = await _factory.CreateDbContextAsync();
			await dbContext.Database.EnsureCreatedAsync();
		}

		[Benchmark(Baseline = true)]
		public ValueTask<Subject> Measure() => _single.Get();

		[Benchmark]
		public async Task<Subject> MeasureCompiled()
		{
			await using var context = await _factory.CreateDbContextAsync();
			var             result  = await _create(context);
			return result;
		}
	}
}