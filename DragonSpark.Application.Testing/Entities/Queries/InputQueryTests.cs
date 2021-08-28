using DragonSpark.Application.Entities.Queries;
using DragonSpark.Compose;
using DragonSpark.Runtime.Execution;
using DragonSpark.Testing.Objects.Entities;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
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
			var factory = new CounterAwareDbContexts<Context>(new InMemoryDbContexts<Context>(), counter);
			{
				await using var context = factory.CreateDbContext();
				context.Subjects.AddRange(new Subject { Name = "One" }, new Subject { Name = "Two" },
				                          new Subject { Name = "Three" });
				await context.SaveChangesAsync();
			}

			counter.Get().Should().Be(1);

			var evaluate =
				new EvaluateToArray<Context, string, string>(new DbContexts<Context>(factory), Selected.Default);
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

			var evaluation = new EvaluateToArray<Context, string, string>(factory, Selected.Default);
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
				new CounterAwareDbContexts<ContextWithData>(new InMemoryDbContexts<ContextWithData>(), counter);
			{
				await using var context = contexts.CreateDbContext();
				await context.Database.EnsureCreatedAsync();
			}

			counter.Get().Should().Be(1);

			var id = new Guid("C750443A-19D0-4FD0-B45A-9D1722AD0DB3");

			var evaluate =
				new EvaluateToArray<ContextWithData, (Guid, string), string>(new DbContexts<ContextWithData>(contexts),
				                                                             ComplexSelected.Default);
			{
				var results = await evaluate.Await(new (id, "One"));
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

			var evaluate = new EvaluateToArray<ContextWithData, (Guid, string), string>(contexts, ComplexSelected.Default);
			{
				var results = await evaluate.Await(new (id, "One"));
				var only    = results.Open().Only();
				only.Should().NotBeNull();
				only.Should().Be("One");
			}
		}

		sealed class Context : DbContext
		{
			public Context(DbContextOptions options) : base(options) {}

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

			public string Name { get; set; } = default!;
		}

		sealed class Selected : InputQuery<string, Subject, string>
		{
			public static Selected Default { get; } = new Selected();

			Selected() : base((s, queryable) => queryable.Where(y => y.Name != s).Select(y => y.Name)) {}
		}

		public readonly record struct Input(Guid Identity, string Name);
/*public readonly struct Input
		{
			public Input(Guid identity, string name)
			{
				Identity = identity;
				Name     = name;
			}

			public Guid Identity { get; }

			public string Name { get; }

			public void Deconstruct(out Guid identity, out string name)
			{
				identity = Identity;
				name     = Name;
			}
		}*/

		sealed class ComplexSelected : InputQuery<(Guid, string), Subject, string>
		{
			public static ComplexSelected Default { get; } = new();

			ComplexSelected()
				: base((input, queryable)
					       => queryable.Where(y => y.Id == input.Item1 && y.Name == input.Item2)
					                   .Select(y => y.Name)) {}
		}
	}
}