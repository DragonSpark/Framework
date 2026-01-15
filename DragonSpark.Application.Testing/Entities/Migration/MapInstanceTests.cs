using DragonSpark.Application.AspNet.Entities.Migration;
using DragonSpark.Runtime;
using DragonSpark.Testing.Objects.Entities.SqlLite;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Xunit;

namespace DragonSpark.Application.Testing.Entities.Migration;

public sealed class MapInstanceTests
{
	[Fact]
	public async Task VerifyBasicMappingWorksAsExpected()
	{
		await using var sources      = await new SqlLiteNewContext<FromContext>().Initialize();
		await using var destinations = await new SqlLiteNewContext<ToContext>().Initialize();

		var subject = MapEntries.Default;

		{
			await using var seed = sources.Get();
			seed.Basic.AddRange(new() { Name = "One", Created   = Time.Default },
			                    new() { Name = "Two", Created   = Time.Default },
			                    new() { Name = "Three", Created = Time.Default });
			await seed.SaveChangesAsync();
		}

		{
			await using var source      = sources.Get();
			await using var destination = destinations.Get();

			foreach (var from in source.Basic)
			{
				subject.Execute(MapInput.New<To>(source.Entry(from), destination));
			}

			var changes = await destination.SaveChangesAsync();
			changes.Should().Be(3);
		}

		{
			await using var source      = sources.Get();
			await using var destination = destinations.Get();
			var             count       = await destination.Basic.CountAsync();
			count.Should().Be(3);
			foreach (var to in destination.Basic)
			{
				var from = await source.Basic.SingleAsync(x => x.Id == to.Id);
				from.Should().BeEquivalentTo(to);
				to.Id.Should().Be(from.Id);
			}
		}
	}

	[Fact]
	public async Task VerifyOwnedMappingWorksAsExpected()
	{
		await using var sources      = await new SqlLiteNewContext<FromContext>().Initialize();
		await using var destinations = await new SqlLiteNewContext<ToContext>().Initialize();

		var subject = MapEntries.Default;

		{
			await using var seed = sources.Get();
			seed.Owned.AddRange(new() { Name = "One", Created   = Time.Default, Owned = new() { Message = "First" } },
			                    new() { Name = "Two", Created   = Time.Default, Owned = new() { Message = "Second" } },
			                    new() { Name = "Three", Created = Time.Default, Owned = new() { Message = "Third" } });
			await seed.SaveChangesAsync();
		}

		{
			await using var source      = sources.Get();
			await using var destination = destinations.Get();

			foreach (var from in source.Owned)
			{
				subject.Execute(MapInput.New<ToOwned>(source.Entry(from), destination));
			}

			var changes = await destination.SaveChangesAsync();
			changes.Should().Be(3);
		}

		{
			await using var source      = sources.Get();
			await using var destination = destinations.Get();
			var             count       = await destination.Owned.CountAsync();
			count.Should().Be(3);
			foreach (var to in destination.Owned)
			{
				var from = await source.Owned.SingleAsync(x => x.Id == to.Id);
				from.Should().BeEquivalentTo(to);
				to.Id.Should().Be(from.Id);
				from.Owned.Should().BeEquivalentTo(to.Owned);
			}
		}
	}


	/*sealed class ModelTypes : AspNet.Entities.Migration.ModelTypes
	{
		public static ModelTypes Default { get; } = new();

		ModelTypes() : base(new ForwardedType(typeof(From), typeof(To))) {}
	}*/

	sealed class FromContext : DbContext
	{
		public FromContext(DbContextOptions options) : base(options) {}

		public required DbSet<From> Basic { get; [UsedImplicitly] init; }

		public required DbSet<FromOwned> Owned { get; [UsedImplicitly] init; }
	}

	sealed class From
	{
		public uint Id { get; init; }

		public required DateTimeOffset Created { get; init; }

		[MaxLength(16)]
		public required string Name { get; init; }
	}

	sealed class FromOwned
	{
		public Guid Id { get; init; }

		public required DateTimeOffset Created { get; init; }

		[MaxLength(16)]
		public required string Name { get; init; }

		public required FromOwnedValue Owned { get; set; }
	}

	[Owned]
	sealed class FromOwnedValue
	{
		[MaxLength(64), UsedImplicitly]
		public required string Message { get; set; }
	}

	/**/

	sealed class ToContext : DbContext
	{
		public ToContext(DbContextOptions options) : base(options) {}

		public required DbSet<To> Basic { get; [UsedImplicitly] init; }

		public required DbSet<ToOwned> Owned { get; [UsedImplicitly] set; }
	}

	sealed class To
	{
		public uint Id { get; init; }

		public required DateTimeOffset Created { get; init; }

		[MaxLength(16)]
		public required string Name { get; init; }
	}

	sealed class ToOwned
	{
		public Guid Id { get; init; }

		public required DateTimeOffset Created { get; init; }

		[MaxLength(16)]
		public required string Name { get; init; }

		public required ToOwnedValue Owned { get; set; }
	}

	[Owned]
	sealed class ToOwnedValue
	{
		[MaxLength(64), UsedImplicitly]
		public required string Message { get; set; }
	}
}