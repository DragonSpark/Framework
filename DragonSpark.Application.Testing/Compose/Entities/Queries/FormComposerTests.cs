using BenchmarkDotNet.Attributes;
using DragonSpark.Application.AspNet;
using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Testing.Objects.Entities;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DragonSpark.Application.Testing.Compose.Entities.Queries;

public sealed class FormComposerTests
{
	[Fact]
	public async Task VerifyArray()
	{
		var contexts = new MemoryNewContext<Context>();
		{
			await using var context = contexts.Get();
			await context.Database.EnsureCreatedAsync();
		}
		{
			var result = await Start.A.Query<Subject>()
			                        .Where(x => x.Name != "Two")
			                        .Invoke(contexts)
			                        .To.Array()
			                        .Get(CancellationToken.None);
			result.Length.Should().Be(2);
			result.Open().Select(x => x.Name).Should().BeEquivalentTo("One", "Three");
		}
	}

	[Fact]
	public async Task VerifyLease()
	{
		var contexts = new NewContext<Context>(new InMemoryDbContextFactory<Context>());
		{
			await using var context = contexts.Get();
			await context.Database.EnsureCreatedAsync();
		}
		{
			using var result =
				await Start.A.Query<Subject>().Where(x => x.Name != "Two").Invoke(contexts).To.Lease().Get(CancellationToken.None);
			result.Length.Should().Be(2);
			result.ToArray().Select(x => x.Name).Should().BeEquivalentTo("One", "Three");
		}
	}

	[Fact]
	public async Task VerifyList()
	{
		var contexts = new NewContext<Context>(new InMemoryDbContextFactory<Context>());
		{
			await using var context = contexts.Get();
			await context.Database.EnsureCreatedAsync();
		}
		{
			var result = await Start.A.Query<Subject>()
			                        .Where(x => x.Name != "Two")
			                        .Invoke(contexts)
			                        .To.List()
			                        .Get(CancellationToken.None);
			result.Count.Should().Be(2);
			result.Select(x => x.Name).Should().BeEquivalentTo("One", "Three");
		}
	}

	[Fact]
	public async Task VerifyDictionary()
	{
		var contexts = new NewContext<Context>(new InMemoryDbContextFactory<Context>());
		{
			await using var context = contexts.Get();
			await context.Database.EnsureCreatedAsync();
		}
		{
			var result = await Start.A.Query<Subject>()
			                        .Where(x => x.Name != "Two")
			                        .Invoke(contexts)
			                        .To.Dictionary(x => x.Name)
			                        .Get(CancellationToken.None);
			result.Count.Should().Be(2);
			result.Keys.Should().BeEquivalentTo("One", "Three");
		}
	}

	[Fact]
	public async Task VerifyDictionaryValues()
	{
		var contexts = new NewContext<Context>(new InMemoryDbContextFactory<Context>());
		{
			await using var context = contexts.Get();
			await context.Database.EnsureCreatedAsync();
		}
		{
			var result = await Start.A.Query<Subject>()
			                        .Where(x => x.Name != "Two")
			                        .Invoke(contexts)
			                        .To.Dictionary(x => x.Name, x => x.Id)
			                        .Get(CancellationToken.None);
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
		var contexts = new NewContext<Context>(new InMemoryDbContextFactory<Context>());
		{
			await using var context = contexts.Get();
			await context.Database.EnsureCreatedAsync();
		}
		{
			var single = await Start.A.Query<Subject>()
			                        .Where(x => x.Name == "Two")
			                        .Invoke(contexts)
			                        .To.Single()
			                        .Get(CancellationToken.None);
			single.Should().NotBeNull();
		}
		{
			var single = Start.A.Query<Subject>().Where(x => x.Name != "Two").Invoke(contexts).To.Single().Get(CancellationToken.None);
			await single.Awaiting(x => x.AsTask()).Should().ThrowAsync<InvalidOperationException>();
		}
	}

	[Fact]
	public async Task VerifySingleOrDefault()
	{
		var contexts = new NewContext<Context>(new InMemoryDbContextFactory<Context>());
		{
			await using var context = contexts.Get();
			await context.Database.EnsureCreatedAsync();
		}
		{
			var single = await Start.A.Query<Subject>()
			                        .Where(x => x.Name == "Two")
			                        .Invoke(contexts)
			                        .To.SingleOrDefault()
			                        .Get(CancellationToken.None);
			single.Should().NotBeNull();
		}
		{
			var single = await Start.A.Query<Subject>()
			                        .Where(x => x.Name == "Four")
			                        .Invoke(contexts)
			                        .To.SingleOrDefault()
			                        .Get(CancellationToken.None);
			single.Should().BeNull();
		}
	}

	[Fact]
	public async Task VerifyFirst()
	{
		var contexts = new MemoryNewContext<Context>();
		{
			await using var context = contexts.Get();
			await context.Database.EnsureCreatedAsync();
		}
		{
			var single = await Start.A.Query<Subject>()
			                        .Where(x => x.Name == "Two")
			                        .Invoke(contexts)
			                        .To.First()
			                        .Get(CancellationToken.None);
			single.Should().NotBeNull();
		}
		{
			var single = Start.A.Query<Subject>().Where(x => x.Name == "Four").Invoke(contexts).To.First().Get(CancellationToken.None);
			await single.Awaiting(x => x.AsTask()).Should().ThrowAsync<InvalidOperationException>();
		}
	}

	[Fact]
	public async Task VerifyFirstOrDefault()
	{
		var contexts = new NewContext<Context>(new InMemoryDbContextFactory<Context>());
		{
			await using var context = contexts.Get();
			await context.Database.EnsureCreatedAsync();
		}
		{
			var single = await Start.A.Query<Subject>()
			                        .Where(x => x.Name == "Two")
			                        .Invoke(contexts)
			                        .To.FirstOrDefault()
			                        .Get(CancellationToken.None);
			single.Should().NotBeNull();
		}
		{
			var single = await Start.A.Query<Subject>()
			                        .Where(x => x.Name == "Four")
			                        .Invoke(contexts)
			                        .To.FirstOrDefault()
			                        .Get(CancellationToken.None);
			single.Should().BeNull();
		}
	}

	[Fact]
	public async Task VerifyAny()
	{
		var contexts = new NewContext<Context>(new InMemoryDbContextFactory<Context>());
		{
			await using var context = contexts.Get();
			await context.Database.EnsureCreatedAsync();
		}
		{
			var single = await Start.A.Query<Subject>().Where(x => x.Name == "Two").Invoke(contexts).To.Any().Get(CancellationToken.None);
			single.Should().BeTrue();
		}
		{
			var single = await Start.A.Query<Subject>()
			                        .Where(x => x.Name == "Four")
			                        .Invoke(contexts)
			                        .To.Any()
			                        .Get(CancellationToken.None);
			single.Should().BeFalse();
		}
	}

	sealed class Context : DbContext
	{
		public Context(DbContextOptions options) : base(options) {}

		[UsedImplicitly]
		public DbSet<Subject> Subjects { get; set; } = null!;

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

		[MaxLength(64)]
		public string Name { get; set; } = null!;
	}

	public class Benchmarks
	{
		readonly IStopAware<None, Subject>      _single;
		readonly IDbContextFactory<Context>     _factory;
		readonly Func<DbContext, Task<Subject>> _create;

		public Benchmarks() : this(new InMemoryDbContextFactory<Context>()) {}

		Benchmarks(IDbContextFactory<Context> factory) : this(new NewContext<Context>(factory), factory) {}

		Benchmarks(INewContext<Context> @new, IDbContextFactory<Context> factory)
			: this(Start.A.Query<Subject>().Where(x => x.Name == "Two").Invoke(@new).To.Single(),
			       factory,
			       EF.CompileAsyncQuery<DbContext, Subject>(x => x.Set<Subject>().Single(y => y.Name == "Two"))) {}

		Benchmarks(IStopAware<None, Subject> single, IDbContextFactory<Context> factory,
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
		public ValueTask<Subject> Measure() => _single.Get(CancellationToken.None);

		[Benchmark]
		public async Task<Subject> MeasureCompiled()
		{
			await using var context = await _factory.CreateDbContextAsync();
			var             result  = await _create(context);
			return result;
		}
	}
}