using DragonSpark.Application.AspNet.Entities.Migration;
using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;
using DragonSpark.Compose;
using DragonSpark.Runtime;
using DragonSpark.Testing.Objects.Entities.SqlLite;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DragonSpark.Application.Testing.Entities.Migration;

public sealed class MapTests
{
	[Fact]
	public async Task VerifyBasicMappingWorksAsExpected()
	{
		await using var sources      = await new SqlLiteNewContext<FromContext>().Initialize();
		await using var destinations = await new SqlLiteNewContext<ToContext>().Initialize();

		var subject = Map.Default;

		{
			await using var seed = sources.Get();
			seed.Basic.AddRange(new() { Name = "One", Created   = Time.Default, Enumeration = FromEnum.Four},
			                    new() { Name = "Two", Created   = Time.Default, Enumeration = FromEnum.Two },
			                    new() { Name = "Three", Created = Time.Default, Enumeration = FromEnum.One });
			await seed.SaveChangesAsync();
		}

		{
			await using var source      = sources.Get();
			await using var destination = destinations.Get();

			foreach (var from in source.Basic)
			{
				await subject.Off(new(MapInput.New<To>(source.Entry(from), destination), CancellationToken.None));
			}

			var changes = await destination.SaveChangesAsync();
			changes.Should().Be(3);
		}
		
		{
			await using var source      = sources.Get();
			await using var destination = destinations.Get();

			foreach (var from in source.Basic)
			{
				await subject.Off(new(MapInput.New<To>(source.Entry(from), destination), CancellationToken.None));
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
				Convert.ToByte(to.Enumeration).Should().Be(Convert.ToByte(from.Enumeration));
			}
		}
	}

	[Fact]
	public async Task VerifyOwnedMappingWorksAsExpected()
	{
		await using var sources      = await new SqlLiteNewContext<FromContext>().Initialize();
		await using var destinations = await new SqlLiteNewContext<ToContext>().Initialize();

		var subject = Map.Default;

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
				await subject.Off(new(MapInput.New<ToOwned>(source.Entry(from), destination), CancellationToken.None));
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

	[Fact]
	public async Task VerifyAssociatedMappingWorksAsExpected()
	{
		await using var sources      = await new SqlLiteNewContext<FromContext>().Initialize();
		await using var destinations = await new SqlLiteNewContext<ToContext>().Initialize();

		var subject = Map.Default;

		{
			await using var seed = sources.Get();
			seed.Associated.AddRange(new()
			                         {
				                         Name = "One", Created = Time.Default, Association = new() { Message = "First" }
			                         },
			                         new()
			                         {
				                         Name        = "Two", Created = Time.Default,
				                         Association = new() { Message = "Second" }
			                         },
			                         new()
			                         {
				                         Name        = "Three", Created = Time.Default,
				                         Association = new() { Message = "Third" }
			                         });
			await seed.SaveChangesAsync();
		}

		{
			await using var source      = sources.Get();
			await using var destination = destinations.Get();

			foreach (var from in source.Associations)
			{
				await subject.Off(new(MapInput.New<ToAssociation>(source.Entry(from), destination),
				                      CancellationToken.None));
			}

			foreach (var from in source.Associated)
			{
				await subject.Off(new(MapInput.New<ToAssociated>(source.Entry(from), destination),
				                      CancellationToken.None));
			}

			var changes = await destination.SaveChangesAsync();
			changes.Should().Be(6);
		}

		{
			await using var source      = sources.Get();
			await using var destination = destinations.Get();
			var             count       = await destination.Associations.CountAsync();
			count.Should().Be(3);
			foreach (var to in destination.Associations)
			{
				var from = await source.Associations.SingleAsync(x => x.Id == to.Id);
				from.Should().BeEquivalentTo(to);
				to.Id.Should().Be(from.Id);
			}
		}

		{
			await using var source      = sources.Get();
			await using var destination = destinations.Get();
			var             count       = await destination.Associated.CountAsync();
			count.Should().Be(3);
			foreach (var to in destination.Associated.Include(x => x.Association))
			{
				var from = await source.Associated.Include(x => x.Association).SingleAsync(x => x.Id == to.Id);
				from.Should().BeEquivalentTo(to);
				to.Id.Should().Be(from.Id);
				from.Association.Should().NotBeNull();
				from.Association.Should().BeEquivalentTo(to.Association);
			}
		}
	}

	sealed class FromContext : DbContext
	{
		public FromContext(DbContextOptions options) : base(options) {}

		public required DbSet<From> Basic { get; [UsedImplicitly] init; }

		public required DbSet<FromOwned> Owned { get; [UsedImplicitly] init; }

		public required DbSet<FromAssociated> Associated { get; [UsedImplicitly] init; }

		public required DbSet<FromAssociation> Associations { get; [UsedImplicitly] init; }
	}

	sealed class From
	{
		public uint Id { get; init; }

		public required DateTimeOffset Created { get; init; }

		[MaxLength(16)]
		public required string Name { get; init; }

		public required FromEnum Enumeration { get; init; }
	}

	enum FromEnum : byte { One, Two, [UsedImplicitly] Three, Four }

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

	sealed class FromAssociated
	{
		public Guid Id { get; init; }

		public required DateTimeOffset Created { get; init; }

		[MaxLength(16)]
		public required string Name { get; init; }

		[ForeignKey("AssociationId")]
		public required FromAssociation Association { get; set; }
	}

	sealed class FromAssociation
	{
		public uint Id { get; init; }

		[MaxLength(64), UsedImplicitly]
		public required string Message { get; set; }
	}

	/**/

	sealed class ToContext : DbContext
	{
		public ToContext(DbContextOptions options) : base(options) {}

		public required DbSet<To> Basic { get; [UsedImplicitly] init; }

		public required DbSet<ToOwned> Owned { get; [UsedImplicitly] set; }

		public required DbSet<ToAssociated> Associated { get; [UsedImplicitly] init; }
		public required DbSet<ToAssociation> Associations { get; [UsedImplicitly] init; }
	}

	sealed class To
	{
		public uint Id { get; init; }

		public required DateTimeOffset Created { get; init; }

		[MaxLength(16)]
		public required string Name { get; init; }

		public required ToEnum Enumeration { get; set; }
	}

	enum ToEnum : byte { [UsedImplicitly]One, [UsedImplicitly]Two, [UsedImplicitly]Three, [UsedImplicitly] Four }

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

	sealed class ToAssociated
	{
		public Guid Id { get; init; }

		public required DateTimeOffset Created { get; init; }

		[MaxLength(16)]
		public required string Name { get; init; }

		[ForeignKey("AssociationId")]
		public required ToAssociation Association { get; init; }
	}

	sealed class ToAssociation
	{
		public uint Id { get; init; }

		[MaxLength(64), UsedImplicitly]
		public required string Message { get; set; }
	}
}