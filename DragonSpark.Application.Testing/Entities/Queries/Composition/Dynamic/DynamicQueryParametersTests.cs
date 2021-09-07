using DragonSpark.Application.Entities.Queries.Composition.Dynamic;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Testing.Objects.Entities;
using FluentAssertions;
using JetBrains.Annotations;
using LinqKit;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace DragonSpark.Application.Testing.Entities.Queries.Composition.Dynamic
{
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

			/*{
				await using var context = contexts.Get();
				var temp = await context.Subjects.Where("Name == @0", "One").ToArrayAsync();
				temp.Only().Name.Should().Be("One");
			}*/

			/*LambdaExpression e = DynamicExpressionParser.ParseLambda(
			                                                         typeof(Subject), typeof(bool),
			                                                         "City = @0 and Orders.Count >= @1",
			                                                         "London", 10);*/
			var e =
				(Expression<Func<DynamicQueryParameters, Subject, bool>>)DynamicExpressionParser.ParseLambda(new[]
				{
					Expression.Parameter(typeof(DynamicQueryParameters), "p"),
					Expression.Parameter(typeof(Subject), "x"),
				}, null, "p.Filter == null || x.Name == p.Filter");

			var order =
				(Expression<Func<DynamicQueryParameters, IQueryable<Subject>, IQueryable<Subject>>>)
				DynamicExpressionParser.ParseLambda(new[]
				{
					Expression.Parameter(typeof(DynamicQueryParameters), "order"),
					Expression.Parameter(typeof(IQueryable<Subject>), "q"),
				}, A.Type<IQueryable<Subject>>(), @"q.OrderBy(order.Filter).Select(x => x)");

			/*var sut = contexts.Then()
			                  .Accept<DynamicQueryParameters>()
			                  .Use<Subject>()
			                  /*.Select(x => x.Where(e))
			                  #1#
			                  .Select(x => x.Select(order))
							  /*.Select(q => q.OrderBy(x => x.Name))#1#
			                  /*.Select(q => q.Select(x => x.OrderBy(@"np(Name, """")")))#1#
			                  /*.Select(x => x.Select((c, p, q) => q.OrderBy(@"np(p.OrderBy, """")")))#1#
			                  .To.Array();*/

			{
				/*var found = await sut.Get(new DynamicQueryParameters { Filter = "One" });
				found.Open().Only().Name.Should().Be("One");*/
			}
			{
				await using var context = contexts.Get();
				var query =
					EF.CompileAsyncQuery<DbContext, string, Subject>((c, o) => c.Set<Subject>().OrderBy("Name"));

				var open    = await /*sut.Get(new DynamicQueryParameters {Filter = "Name", OrderBy = "Name ASC"})*/
					query.Invoke(context, "Name ASC").ToArrayAsync();
				open.Should().HaveCount(3);
				open.First().Name.Should().Be("One");
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