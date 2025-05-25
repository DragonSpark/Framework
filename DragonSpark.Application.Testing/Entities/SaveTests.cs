using BenchmarkDotNet.Attributes;
using DragonSpark.Application.AspNet;
using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Application.AspNet.Entities.Editing;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Testing.Objects.Entities;
using DragonSpark.Testing.Objects.Entities.SqlLite;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DragonSpark.Application.Testing.Entities;

public sealed class SaveTests
{
	[Fact]
	public async Task Verify()
	{
		const string original = "Default Name",
		             expected = "Updated Name";

		await using var contexts = await new SqlLiteNewContext<Context>().Initialize();

		{
			await using var data = contexts.Get();
			data.Subjects.Add(new Subject { Name = original });
			await data.SaveChangesAsync();
		}
		var query = contexts.Then().Use<Subject>().To.Single();
		var sut = new Save<Subject>(new EnlistedScopes(new Scopes<Context>(contexts),
		                                               EmptyAmbientContext.Default));
		{
			var first = await query.Off(CancellationToken.None);
			first.Name.Should().Be(original);
			first.Name = expected;
			await sut.Off(new(first, CancellationToken.None));
		}

		{
			var first = await query.Off(CancellationToken.None);
			first.Name.Should().Be(expected);
		}
	}

	[Fact]
	public async Task VerifyRelationship()
	{
		await using var contexts = await new SqlLiteNewContext<ContextWithRelationship>().Initialize();

		{
			await using var data = contexts.Get();
			data.Update(new First());
			await data.SaveChangesAsync();
		}

		{
			await using var data   = contexts.Get();
			var             first  = await data.Firsts.SingleAsync();
			var             second = new Second() { First = first };
			data.Set<First>().Update(first);
			data.Update(second);
			await data.SaveChangesAsync();
		}
		{
			await using var data  = contexts.Get();
			var             count = await data.Set<Second>().CountAsync();
			count.Should().Be(1);
		}
	}

	sealed class EmptyAmbientContext : Instance<DbContext?>, IAmbientContext
	{
		public static EmptyAmbientContext Default { get; } = new();

		EmptyAmbientContext() : base(null) {}
	}

	sealed class Context : DbContext
	{
		public Context(DbContextOptions options) : base(options) {}

		[UsedImplicitly]
		public DbSet<Subject> Subjects { get; set; } = null!;
	}

	sealed class ContextWithRelationship : DbContext
	{
		public ContextWithRelationship(DbContextOptions options) : base(options) {}

		[UsedImplicitly]
		public DbSet<First> Firsts { get; set; } = null!;

		[UsedImplicitly]
		public DbSet<Second> Seconds { get; set; } = null!;

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Second>()
			            .HasOne(x => x.First)
			            .WithOne()
			            .HasForeignKey<Second>("FirstId")
			            .IsRequired();
			base.OnModelCreating(modelBuilder);
		}
	}

	class Subject
	{
		[UsedImplicitly]
		public virtual Guid Id { get; set; }

		[MaxLength(64)]
		public virtual string Name { [UsedImplicitly] get; set; } = null!;
	}

	class First
	{
		[UsedImplicitly]
		public virtual Guid Id { get; set; }
	}

	class Second
	{
		[UsedImplicitly]
		public virtual Guid Id { get; set; }

		public virtual First? First { get; set; }
	}

	public class Benchmarks
	{
		readonly INewContext<Context> _new;

		public Benchmarks() : this(new PooledMemoryNewContext<Context>()) {}

		Benchmarks(INewContext<Context> @new) => _new = @new;

		[GlobalSetup]
		public async Task GlobalSetup()
		{
			await using var context = _new.Get();
			await context.Database.EnsureDeletedAsync();
			await context.Database.EnsureCreatedAsync();
			context.Subjects.Add(new Subject { Name = "One" });
			await context.SaveChangesAsync();
		}

		[Benchmark(Baseline = true)]
		public async Task<object> MeasureUnit()
		{
			await using var context = _new.Get();
			var             result  = await context.Subjects.SingleAsync();
			result.Name = "Updated";
			await context.SaveChangesAsync();
			return result;
		}

		[Benchmark]
		public async Task<object> MeasureAttach()
		{
			await using var context = _new.Get();
			var             result  = await context.Subjects.AsNoTracking().SingleAsync();
			context.Subjects.Attach(result);
			result.Name = "Updated";
			await context.SaveChangesAsync();
			return result;
		}

		[Benchmark]
		public async Task<object> MeasureSelectAndSave()
		{
			var             result  = await Select();
			await using var context = _new.Get();
			context.Subjects.Attach(result);

			result.Name = "Updated";
			await context.SaveChangesAsync();
			return result;
		}

		async Task<Subject> Select()
		{
			await using var context = _new.Get();
			return await context.Subjects.SingleAsync();
		}
	}

	public class AmbientBenchmarks
	{
		readonly CancellationTokenSource source = new();
		[GlobalSetup]
		public void GlobalSetup()
		{
			AmbientToken.Default.Execute(source.Token);

		}

		/*[Benchmark]
		public Task Assign()
		{
			using var _ = AmbientToken.Default.Assigned(source.Token);
			return Task.CompletedTask;
		}*/

		[Benchmark]
		public Task Read()
		{
			var token = AmbientToken.Default.Get();
			if (token == CancellationToken.None)
			{
				throw new InvalidOperationException();
			}
			return Task.CompletedTask;
		}
	}


	public class DisposeBenchmarks
	{
		readonly INewContext<Context> _new;

		public DisposeBenchmarks() : this(new PooledMemoryNewContext<Context>()) {}

		DisposeBenchmarks(INewContext<Context> @new) => _new = @new;

		[GlobalSetup]
		public async Task GlobalSetup()
		{
			await using var context = _new.Get();
			await context.Database.EnsureDeletedAsync();
			await context.Database.EnsureCreatedAsync();
			context.Subjects.Add(new Subject { Name = "One" });
			await context.SaveChangesAsync();
		}

		[Benchmark(Baseline = true)]
		public void Synchronous()
		{
			using var context = _new.Get();
		}

		[Benchmark]
		public async ValueTask Operation()
		{
			await using var context = _new.Get();
		}

		[Benchmark]
		public ValueTask SynchronousOperation()
		{
			using var context = _new.Get();
			return ValueTask.CompletedTask;
		}
	}
}