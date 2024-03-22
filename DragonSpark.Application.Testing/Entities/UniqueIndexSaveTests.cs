using DragonSpark.Testing.Objects.Entities.Sql;
using EntityFramework.Exceptions.SqlServer;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Xunit;

namespace DragonSpark.Application.Testing.Entities
{
	public sealed class UniqueIndexSaveTests
	{
		[Fact]
		public async Task Verify()
		{
			await using var contexts = await new SqlContexts<SubjectContext>().Initialize();
			{
				await using var data = contexts.Get();
				data.Set<Authenticator>().AddRange(new Authenticator { Identifier = "Twitter", Name = "Twitter" },
				                                   new Authenticator{ Identifier  = "Twitter", Name = "Twitter" });
				await data.SaveChangesAsync();
			}
		}

		sealed class Context : DbContext
		{
			public Context(DbContextOptions options) : base(options) {}

		}

		sealed class SubjectContext : DbContext
		{
			public SubjectContext(DbContextOptions options) : base(options) {}

			protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
			{
				base.OnConfiguring(optionsBuilder.UseExceptionProcessor());
			}

			protected override void OnModelCreating(ModelBuilder modelBuilder)
			{
				modelBuilder.Entity<Authenticator>();
				modelBuilder.Entity<Transaction>();
				modelBuilder.Entity<ExternalProcess>();
				modelBuilder.Entity<PurchaseOrder>();
				modelBuilder.Entity<SaleOrder>();
				modelBuilder.Entity<DepositOrder>(x =>
				                                  {
					                                  x.HasOne(y => y.Result)
					                                   .WithOne(y => y.Order)
					                                   .HasForeignKey<DepositOrder>();
				                                  });

				modelBuilder.Entity<WithdrawOrder>(x =>
				                                   {
					                                   x.HasOne(y => y.Result)
					                                    .WithOne(y => y.Order)
					                                    .HasForeignKey<WithdrawOrder>("ResultId")
					                                    .IsRequired(false);
					                                   x.HasIndex("ResultId")
					                                    .IsUnique()
					                                    .HasFilter("[WithdrawOrder_ResultId] IS NOT NULL");
				                                   });

				base.OnModelCreating(modelBuilder);
			}
		}

		public abstract class PayableOrder : ExternalProcess
		{

		}
		public abstract class Debit : Transaction;
		public class Withdrawal : Debit
		{
			public WithdrawOrder Order { get; set; } = default!;

		}

		public sealed class WithdrawOrder : PayableOrder
		{
			public Withdrawal? Result { get; set; }


			public decimal Charges { get; set; }
		}

		[Index(nameof(Identifier), IsUnique = true)]
		public class Authenticator
		{
			public Guid Id { get; set; }

			public bool Enabled { get; set; } = true;

			public DateTimeOffset Created { get; set; }

			[MaxLength(32)]
			public string Name { get; set; } = default!;

			[MaxLength(32)]
			public string Identifier { get; set; } = default!;


		}

		public abstract class ExternalProcess
		{
			public Guid Id { get; set; }

			public bool Enabled { get; set; } = true;

			public DateTimeOffset Created { get; set; }

			public DateTimeOffset? Completed { get; set; }
		}

		public sealed class DepositOrder : ExternalProcess
		{
			public Deposit? Result { get; set; }
		}

		public class Deposit : Credit
		{
			public DepositOrder Order { get; set; } = default!;
		}

		public abstract class Credit : Transaction;

		[Index(nameof(Created))]
		public abstract class Transaction
		{
			public Guid Id { get; set; }

			public DateTimeOffset Created { get; set; }

		}


		public abstract class PurchaseOrder : ExternalProcess
		{
			public MarketplaceTransaction? Result { get; set; }
		}
		public sealed class SaleOrder : PurchaseOrder;
		public sealed class MarketplaceTransaction
		{
			public Guid Id { get; init; }

			public DateTimeOffset Created { get; init; }

			public decimal Value { get; init; }
		}


	}
}
