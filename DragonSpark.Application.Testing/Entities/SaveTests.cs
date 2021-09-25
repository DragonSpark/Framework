using BenchmarkDotNet.Attributes;
using DragonSpark.Application.Entities;
using DragonSpark.Application.Entities.Editing;
using DragonSpark.Compose;
using DragonSpark.Testing.Objects.Entities;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DragonSpark.Application.Testing.Entities
{
	public sealed class SaveTests
	{
		[Fact]
		public async Task Verify()
		{
			const string original = "Default Name",
			             expected = "Updated Name";

			await using var contexts = await new SqlContexts<Context>().Initialize();
			{
				await using var data = contexts.Get();
				data.Subjects.Add(new Subject { Name = original });
				await data.SaveChangesAsync();
			}
			var query = contexts.Then().Use<Subject>().To.Single();
			var sut   = new Save<Context, Subject>(contexts);
			{
				var first = await query.Await();
				first.Name.Should().Be(original);
				first.Name = expected;
				await sut.Await(first);
			}

			{
				var first = await query.Await();
				first.Name.Should().Be(expected);
			}
		}

		[Fact]
		public async Task VerifyRelationship()
		{
			await using var contexts = await new SqlContexts<ContextWithRelationship>().Initialize();
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
				await using var data   = contexts.Get();
				var count = await data.Set<Second>().CountAsync();
				count.Should().Be(1);
			}

		}

		sealed class Context : DbContext
		{
			public Context(DbContextOptions options) : base(options) {}

			[UsedImplicitly]
			public DbSet<Subject> Subjects { get; set; } = default!;
		}

		sealed class ContextWithRelationship : DbContext
		{
			public ContextWithRelationship(DbContextOptions options) : base(options) {}

			[UsedImplicitly]
			public DbSet<First> Firsts { get; set; } = default!;

			[UsedImplicitly]
			public DbSet<Second> Seconds { get; set; } = default!;

			protected override void OnModelCreating(ModelBuilder modelBuilder)
			{
				modelBuilder.Entity<Second>().HasOne(x => x.First).WithOne().HasForeignKey<Second>("FirstId").IsRequired();
				base.OnModelCreating(modelBuilder);
			}
		}

		class Subject
		{
			[UsedImplicitly]
			public virtual Guid Id { get; set; }

			public virtual string Name { [UsedImplicitly] get; set; } = default!;
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
			readonly IContexts<Context> _contexts;

			public Benchmarks() : this(new PooledMemoryContexts<Context>()) {}

			Benchmarks(IContexts<Context> contexts) => _contexts = contexts;

			[GlobalSetup]
			public async Task GlobalSetup()
			{
				await using var context = _contexts.Get();
				await context.Database.EnsureDeletedAsync();
				await context.Database.EnsureCreatedAsync();
				context.Subjects.Add(new Subject { Name = "One" });
				await context.SaveChangesAsync();
			}

			[Benchmark(Baseline = true)]
			public async Task<object> MeasureUnit()
			{
				await using var context = _contexts.Get();
				var result = await context.Subjects.SingleAsync();
				result.Name = "Updated";
				await context.SaveChangesAsync();
				return result;
			}

			[Benchmark]
			public async Task<object> MeasureAttach()
			{
				await using var context = _contexts.Get();
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
				await using var context = _contexts.Get();
				context.Subjects.Attach(result);

				result.Name = "Updated";
				await context.SaveChangesAsync();
				return result;
			}

			async Task<Subject> Select()
			{
				await using var context = _contexts.Get();
				return await context.Subjects.SingleAsync();
			}
		}

		public class DisposeBenchmarks
		{
			readonly IContexts<Context> _contexts;

			public DisposeBenchmarks() : this(new PooledMemoryContexts<Context>()) {}

			DisposeBenchmarks(IContexts<Context> contexts) => _contexts = contexts;

			[GlobalSetup]
			public async Task GlobalSetup()
			{
				await using var context = _contexts.Get();
				await context.Database.EnsureDeletedAsync();
				await context.Database.EnsureCreatedAsync();
				context.Subjects.Add(new Subject { Name = "One" });
				await context.SaveChangesAsync();
			}

			[Benchmark(Baseline = true)]
			public void Synchronous()
			{
				using var context = _contexts.Get();

			}

			[Benchmark]
			public async ValueTask Operation()
			{
				await using var context = _contexts.Get();
			}

			[Benchmark]
			public ValueTask SynchronousOperation()
			{
				using var context = _contexts.Get();
				return ValueTask.CompletedTask;
			}

		}

	}
}