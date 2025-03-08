using DragonSpark.Application.AspNet;
using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Compose;
using DragonSpark.Testing.Objects.Entities;
using DragonSpark.Testing.Objects.Entities.SqlLite;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DragonSpark.Application.Testing.Compose.Entities.Queries;

public sealed class ContextQueryAdapterTests
{
	[Fact]
	public async Task Verify()
	{
		var contexts = new NewContext<Context>(new InMemoryDbContextFactory<Context>());
		{
			await using var context = contexts.Get();
			context.Subjects.AddRange(new Subject { Name = "One" }, new Subject { Name = "Two" },
			                          new Subject { Name = "Three" });
			await context.SaveChangesAsync();
		}

		var evaluate = Start.A.Query<Subject>().Where(x => x.Name != "Two").Invoke(contexts).To.Array();
		{
			var array = await evaluate.Off();
			var open  = array.Open();
			open.Should().HaveCount(2);
			open.Select(x => x.Name).Should().BeEquivalentTo("One", "Three");
		}
	}

	[Fact]
	public async Task VerifySql()
	{
		await using var contexts = await new SqlLiteNewContext<Context>().Initialize();

		{
			await using var context = contexts.Get();
			context.Subjects.AddRange(new Subject { Name = "One" }, new Subject { Name = "Two" },
			                          new Subject { Name = "Three" });
			await context.SaveChangesAsync();
		}

		var evaluate = Start.A.Query<Subject>().Where(x => x.Name != "Two").Invoke(contexts).To.Array();
		{
			var array = await evaluate.Off();
			var open  = array.Open();
			open.Should().HaveCount(2);
			open.Select(x => x.Name).Should().BeEquivalentTo("One", "Three");
		}

	}

	sealed class Context : DbContext
	{
		public Context(DbContextOptions options) : base(options) {}

		public DbSet<Subject> Subjects { get; set; } = null!;
	}

	sealed class Subject
	{
		[UsedImplicitly]
		public Guid Id { get; set; }

		[MaxLength(64)]
		public string Name { get; set; } = null!;
	}
}