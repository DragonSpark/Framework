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
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace DragonSpark.Application.Testing.Entities.Queries;

public sealed class IntroduceTests
{
	[Fact]
	public async Task Verify()
	{
		var contexts = new NewContext<Context>(new InMemoryDbContextFactory<Context>());
		{
			await using var context = contexts.Get();
			context.Subjects.AddRange(new Subject { Name = "One" }, new Subject { Name = "Two" },
			                          new Subject { Name = "Three" });
			context.Others.AddRange(new Other { Name = "One" }, new Other { Name = "Three" });
			await context.SaveChangesAsync();
		}

		var evaluate = contexts.Then().Use(Query.Default).To.Array();
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
		await using var contexts = await new SqlLiteNewContext<Context>().Initialize();

		{
			await using var context = contexts.Get();
			context.Subjects.AddRange(new Subject { Name = "One" }, new Subject { Name = "Two" },
			                          new Subject { Name = "Three" });
			context.Others.AddRange(new Other { Name = "One" }, new Other { Name = "Three" });
			await context.SaveChangesAsync();
		}

		var evaluate = contexts.Then().Use(Query.Default).To.Array();
		{
			var array = await evaluate.Await();
			var open  = array.Open();
			open.Should().HaveCount(2);
			open.Select(x => x.Name).Should().BeEquivalentTo("One", "Three");
		}
	}

	[Fact]
	public async Task VerifyCompose()
	{
		var contexts = new NewContext<Context>(new InMemoryDbContextFactory<Context>());
		{
			await using var context = contexts.Get();
			context.Subjects.AddRange(new Subject { Name = "One" }, new Subject { Name = "Two" },
			                          new Subject { Name = "Three" });
			context.Others.AddRange(new Other { Name = "One" }, new Other { Name = "Three" });
			await context.SaveChangesAsync();
		}

		var evaluate = Start.A.Query<Subject>()
		                    .Introduce(OtherNames.Default)
		                    .Select((_, subjects, names) => subjects.Where(x => names.Contains(x.Name)))
		                    .Invoke(contexts)
		                    .To.Array();
		{
			var array = await evaluate.Await();
			var open  = array.Open();
			open.Should().HaveCount(2);
			open.Select(x => x.Name).Should().BeEquivalentTo("One", "Three");
		}
	}

	[Fact]
	public async Task VerifyComposeSql()
	{
		await using var contexts = await new SqlLiteNewContext<Context>().Initialize();

		{
			await using var context = contexts.Get();
			context.Subjects.AddRange(new Subject { Name = "One" }, new Subject { Name = "Two" },
			                          new Subject { Name = "Three" });
			context.Others.AddRange(new Other { Name = "One" }, new Other { Name = "Three" });
			await context.SaveChangesAsync();
		}

		var evaluate = Start.A.Query<Subject>()
		                    .Introduce(OtherNames.Default)
		                    .Select((_, subjects, names) => subjects.Where(x => names.Contains(x.Name)))
		                    .Invoke(contexts)
		                    .To.Array();
		{
			var array = await evaluate.Await();
			var open  = array.Open();
			open.Should().HaveCount(2);
			open.Select(x => x.Name).Should().BeEquivalentTo("One", "Three");
		}
	}

	[Fact]
	public async Task VerifyComposeTwo()
	{
		var contexts = new NewContext<Context>(new InMemoryDbContextFactory<Context>());
		{
			await using var context = contexts.Get();
			context.Subjects.AddRange(new Subject { Name = "One", Amount   = 5 },
			                          new Subject { Name = "One", Amount   = 10 },
			                          new Subject { Name = "One", Amount   = 15 },
			                          new Subject { Name = "Two", Amount   = 5 },
			                          new Subject { Name = "Two", Amount   = 10 },
			                          new Subject { Name = "Two", Amount   = 15 },
			                          new Subject { Name = "Three", Amount = 5 },
			                          new Subject { Name = "Three", Amount = 10 },
			                          new Subject { Name = "Three", Amount = 15 }
			                         );

			context.Others.AddRange(new Other { Name   = "One" }, new Other { Name = "Three" });
			context.Values.AddRange(new Value { Amount = 5 }, new Value { Amount   = 15 });

			await context.SaveChangesAsync();
		}

		var evaluate = Start.A.Query<Subject>()
		                    .Introduce(OtherNames.Default, Amounts.Default)
		                    .Select((_, subjects, names, amounts)
			                            => subjects.Where(x => names.Contains(x.Name))
			                                       .Where(x => amounts.Contains(x.Amount)))
		                    .Invoke(contexts)
		                    .To.Array();
		{
			var array = await evaluate.Await();
			var open  = array.Open();
			open.Should().HaveCount(4);
			open.Select(x => (x.Name, x.Amount))
			    .Should()
			    .BeEquivalentTo(new[] { ("One", 5u), ("One", 15u), ("Three", 5u), ("Three", 15u) });
		}
	}

	[Fact]
	public async Task VerifyComposeTwoSql()
	{
		await using var contexts = await new SqlLiteNewContext<Context>().Initialize();

		{
			await using var context = contexts.Get();
			context.Subjects.AddRange(new Subject { Name = "One", Amount   = 5 },
			                          new Subject { Name = "One", Amount   = 10 },
			                          new Subject { Name = "One", Amount   = 15 },
			                          new Subject { Name = "Two", Amount   = 5 },
			                          new Subject { Name = "Two", Amount   = 10 },
			                          new Subject { Name = "Two", Amount   = 15 },
			                          new Subject { Name = "Three", Amount = 5 },
			                          new Subject { Name = "Three", Amount = 10 },
			                          new Subject { Name = "Three", Amount = 15 }
			                         );

			context.Others.AddRange(new Other { Name   = "One" }, new Other { Name = "Three" });
			context.Values.AddRange(new Value { Amount = 5 }, new Value { Amount   = 15 });

			await context.SaveChangesAsync();
		}

		var evaluate = Start.A.Query<Subject>()
		                    .Introduce(OtherNames.Default, Amounts.Default)
		                    .Select((_, subjects, names, amounts)
			                            => subjects.Where(x => names.Contains(x.Name))
			                                       .Where(x => amounts.Contains(x.Amount)))
		                    .Invoke(contexts)
		                    .To.Array();
		{
			var array = await evaluate.Await();
			var open  = array.Open();
			open.Should().HaveCount(4);
			open.Select(x => (x.Name, x.Amount))
			    .Should()
			    .BeEquivalentTo(new[] { ("One", 5u), ("One", 15u), ("Three", 5u), ("Three", 15u) });
		}
	}

	[Fact]
	public async Task VerifyComposeThree()
	{
		var contexts = new NewContext<Context>(new InMemoryDbContextFactory<Context>());
		{
			await using var context = contexts.Get();
			context.Subjects.AddRange(new Subject { Name = "One", Amount   = 05, ThirdAmount = -.5f },
			                          new Subject { Name = "One", Amount   = 10, ThirdAmount = 0000 },
			                          new Subject { Name = "One", Amount   = 15, ThirdAmount = 0.5f },
			                          new Subject { Name = "Two", Amount   = 05, ThirdAmount = -.5f },
			                          new Subject { Name = "Two", Amount   = 10, ThirdAmount = 0000 },
			                          new Subject { Name = "Two", Amount   = 15, ThirdAmount = 0.5f },
			                          new Subject { Name = "Three", Amount = 05, ThirdAmount = -.5f },
			                          new Subject { Name = "Three", Amount = 10, ThirdAmount = 0000 },
			                          new Subject { Name = "Three", Amount = 15, ThirdAmount = 0.5f }
			                         );

			context.Others.AddRange(new Other { Name   = "One" }, new Other { Name  = "Three" });
			context.Values.AddRange(new Value { Amount = 5 }, new Value { Amount    = 15 });
			context.Thirds.AddRange(new Third { Amount = -.5f }, new Third { Amount = .5f });

			await context.SaveChangesAsync();
		}

		var evaluate = Start.A.Query<Subject>()
		                    .Introduce(OtherNames.Default, Amounts.Default, ThirdAmounts.Default)
		                    .Select((_, subjects, names, amounts, thirds)
			                            => subjects.Where(x => names.Contains(x.Name))
			                                       .Where(x => amounts.Contains(x.Amount))
			                                       .Where(x => thirds.Contains(x.ThirdAmount)))
		                    .Invoke(contexts)
		                    .To.Array();
		{
			var array = await evaluate.Await();
			var open  = array.Open();
			open.Should().HaveCount(4);
			open.Select(x => (x.Name, x.Amount, Third: x.ThirdAmount))
			    .Should()
			    .BeEquivalentTo(new[]
			    {
				    ("One", 5u, -.5f), ("One", 15u, .5f), ("Three", 5u, -.5f), ("Three", 15u, .5f)
			    });
		}
	}

	[Fact]
	public async Task VerifyComposeThreeSql()
	{
		await using var contexts = await new SqlLiteNewContext<Context>().Initialize();

		{
			await using var context = contexts.Get();
			context.Subjects.AddRange(new Subject { Name = "One", Amount   = 05, ThirdAmount = -.5f },
			                          new Subject { Name = "One", Amount   = 10, ThirdAmount = 0000 },
			                          new Subject { Name = "One", Amount   = 15, ThirdAmount = 0.5f },
			                          new Subject { Name = "Two", Amount   = 05, ThirdAmount = -.5f },
			                          new Subject { Name = "Two", Amount   = 10, ThirdAmount = 0000 },
			                          new Subject { Name = "Two", Amount   = 15, ThirdAmount = 0.5f },
			                          new Subject { Name = "Three", Amount = 05, ThirdAmount = -.5f },
			                          new Subject { Name = "Three", Amount = 10, ThirdAmount = 0000 },
			                          new Subject { Name = "Three", Amount = 15, ThirdAmount = 0.5f }
			                         );

			context.Others.AddRange(new Other { Name   = "One" }, new Other { Name  = "Three" });
			context.Values.AddRange(new Value { Amount = 5 }, new Value { Amount    = 15 });
			context.Thirds.AddRange(new Third { Amount = -.5f }, new Third { Amount = .5f });

			await context.SaveChangesAsync();
		}

		var evaluate = Start.A.Query<Subject>()
		                    .Introduce(OtherNames.Default, Amounts.Default, ThirdAmounts.Default)
		                    .Select((_, subjects, names, amounts, thirds)
			                            => subjects.Where(x => names.Contains(x.Name))
			                                       .Where(x => amounts.Contains(x.Amount))
			                                       .Where(x => thirds.Contains(x.ThirdAmount)))
		                    .Invoke(contexts)
		                    .To.Array();
		{
			var array = await evaluate.Await();
			var open  = array.Open();
			open.Should().HaveCount(4);
			open.Select(x => (x.Name, x.Amount, Third: x.ThirdAmount))
			    .Should()
			    .BeEquivalentTo(new[]
			    {
				    ("One", 5u, -.5f), ("One", 15u, .5f), ("Three", 5u, -.5f), ("Three", 15u, .5f)
			    });
		}
	}

	sealed class Context : DbContext
	{
		public Context(DbContextOptions options) : base(options) {}

		public DbSet<Subject> Subjects { get; set; } = default!;

		public DbSet<Other> Others { get; set; } = default!;

		public DbSet<Value> Values { get; set; } = default!;

		public DbSet<Third> Thirds { get; set; } = default!;
	}

	sealed class Subject
	{
		[UsedImplicitly]
		public Guid Id { get; set; }

		[MaxLength(64)]
		public string Name { get; set; } = default!;

		public uint Amount { get; set; }

		public float ThirdAmount { get; set; }
	}

	sealed class Other
	{
		[UsedImplicitly]
		public Guid Id { get; set; }

		[MaxLength(64)]
		public string Name { get; set; } = default!;
	}

	sealed class Value
	{
		[UsedImplicitly]
		public Guid Id { get; set; }

		public uint Amount { get; set; }
	}

	sealed class Third
	{
		[UsedImplicitly]
		public Guid Id { get; set; }

		public float Amount { get; set; }
	}

	sealed class OtherNames : StartSelect<Other, string>
	{
		public static OtherNames Default { get; } = new();

		OtherNames() : base(x => x.Name) {}
	}

	sealed class Amounts : StartSelect<Value, uint>
	{
		public static Amounts Default { get; } = new();

		Amounts() : base(x => x.Amount) {}
	}

	sealed class ThirdAmounts : StartSelect<Third, float>
	{
		public static ThirdAmounts Default { get; } = new();

		ThirdAmounts() : base(x => x.Amount) {}
	}

	sealed class Query : StartIntroduce<Subject, string, Subject>
	{
		public static Query Default { get; } = new();

		Query() : this(OtherNames.Default.Then()) {}

		public Query(Expression<Func<DbContext, IQueryable<string>>> other)
			: base(other, (context, subjects, names) => subjects.Where(x => names.Contains(x.Name))) {}
	}
}