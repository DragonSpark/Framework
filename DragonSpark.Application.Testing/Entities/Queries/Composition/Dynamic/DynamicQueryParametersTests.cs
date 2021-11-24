using DragonSpark.Testing.Objects.Entities;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DragonSpark.Application.Testing.Entities.Queries.Composition.Dynamic;

public sealed class DynamicQueryParametersTests
{
	[Fact]
	public async Task Verify()
	{
		await using var contexts = await new SqlContexts<ContextWithData>().Initialize();
		{
			await using var context = contexts.Get();
			await context.Database.EnsureCreatedAsync();
		}
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

	public sealed class Subject
	{
		[UsedImplicitly]
		public Guid Id { get; set; }

		[UsedImplicitly]
		public string Name { get; set; } = default!;

		[UsedImplicitly]
		public int Number { get; set; }
	}
}