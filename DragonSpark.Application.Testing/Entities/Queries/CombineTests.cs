using DragonSpark.Application.Entities;
using DragonSpark.Application.Entities.Queries.Composition;
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

namespace DragonSpark.Application.Testing.Entities.Queries;

public sealed class CombineTests
{
	[Fact]
	public async Task Verify()
	{
		var contexts = new Contexts<Context>(new InMemoryDbContextFactory<Context>());
		{
			await using var context = contexts.Get();
			context.Subjects.AddRange(new Subject { Name = "One" }, new Subject { Name = "Two" },
			                          new Subject { Name = "Three" });
			await context.SaveChangesAsync();
		}

		var evaluate = contexts.Then().Use(Combined.Default).To.Array();
		{
			var array = await evaluate.Await();
			var open  = array.Open();
			open.Should().HaveCount(2);
			open.Should().BeEquivalentTo("One", "Three");
		}
	}

	[Fact]
	public async Task VerifySql()
	{
		await using var factory = await new SqlLiteContexts<Context>().Initialize();
		{
			await using var context = factory.Get();
			context.Subjects.AddRange(new Subject { Name = "One" }, new Subject { Name = "Two" },
			                          new Subject { Name = "Three" });
			await context.SaveChangesAsync();
		}

		var evaluate = factory.Then().Use(Combined.Default).To.Array();
		{
			var array = await evaluate.Await();
			var open  = array.Open();
			open.Should().HaveCount(2);
			open.Should().BeEquivalentTo("One", "Three");
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

	sealed class Start : Start<Subject>
	{
		public static Start Default { get; } = new();

		Start() : base(q => q.Where(x => x.Name != "Two")) {}
	}

	sealed class Combined : Combine<Subject, string>
	{
		public static Combined Default { get; } = new();

		Combined() : base(Start.Default, subjects => subjects.Select(x => x.Name)) {}
	}
}