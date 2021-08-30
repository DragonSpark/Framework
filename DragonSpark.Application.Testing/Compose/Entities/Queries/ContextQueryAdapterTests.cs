using DragonSpark.Application.Entities.Queries;
using DragonSpark.Compose;
using DragonSpark.Testing.Objects.Entities;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DragonSpark.Application.Testing.Compose.Entities.Queries
{
	public sealed class ContextQueryAdapterTests
	{
		[Fact]
		public async Task Verify()
		{
			var contexts = new DbContexts<Context>(new InMemoryDbContextFactory<Context>());
			{
				await using var context = contexts.Get();
				context.Subjects.AddRange(new Subject { Name = "One" }, new Subject { Name = "Two" },
				                          new Subject { Name = "Three" });
				await context.SaveChangesAsync();
			}

			var evaluate = contexts.Then().Use<Subject>().Where(x => x.Name != "Two").To.Array();
			{
				var array = await evaluate.Await();
				var open  = array.Open();
				open.Should().HaveCount(2);
				open.Select(x => x.Name).Should().BeEquivalentTo("One", "Three");
			}
		}

		[Fact]
		public async Task VerifySql()
		{
			await using var contexts = await new SqlContexts<Context>().Initialize();
			{
				await using var context = contexts.Get();
				context.Subjects.AddRange(new Subject { Name = "One" }, new Subject { Name = "Two" },
				                          new Subject { Name = "Three" });
				await context.SaveChangesAsync();
			}

			var evaluate = contexts.Then().Use<Subject>().Where(x => x.Name != "Two").To.Array();
			{
				var array = await evaluate.Await();
				var open  = array.Open();
				open.Should().HaveCount(2);
				open.Select(x => x.Name).Should().BeEquivalentTo("One", "Three");
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
	}
}