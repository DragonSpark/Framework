using DragonSpark.Application.Entities.Queries.Compiled;
using DragonSpark.Model;
using DragonSpark.Testing.Objects.Entities;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace DragonSpark.Application.Testing.Entities.Queries.Compilation
{
	public class CompileTests
	{
		[Fact]
		public async Task Verify()
		{
			await using var contexts = await new SqlContexts<ContextWithData>().Initialize();
			{
				await using var context = contexts.Get();
				await context.Database.EnsureCreatedAsync();
			}

			{
				await using var context = contexts.Get();

				Expression<Func<DbContext, Input, IQueryable<string>>> expression
					= (c, i) => c.Set<Subject>().Where(x => x.Name == i.Name).Select(x => x.Name);

				var sut = ManyCompile<Input, string>.Default.Get(expression);
				sut.Should().NotBeNull();
				var query = await sut.Get(new(context, new Input("One"))).SingleAsync();
				query.Should().Be("One");
			}
		}

		[Fact]
		public async Task VerifyNone()
		{
			await using var contexts = await new SqlContexts<ContextWithData>().Initialize();
			{
				await using var context = contexts.Get();
				await context.Database.EnsureCreatedAsync();
			}

			{
				await using var context = contexts.Get();

				Expression<Func<DbContext, None, IQueryable<string>>> expression
					= (c, i) => c.Set<Subject>().Where(x => x.Name != "Two").Select(x => x.Name);

				var sut = ManyCompile<None, string>.Default.Get(expression);
				sut.Should().NotBeNull();

				var query = await sut.Get(new(context, None.Default)).ToArrayAsync();
				query.Should().BeEquivalentTo("One", "Three");
			}
		}

		readonly record struct Input(string Name);

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

			[UsedImplicitly]
			public string Name { get; set; } = default!;

			[UsedImplicitly]
			public int Number { get; set; }
		}
	}
}