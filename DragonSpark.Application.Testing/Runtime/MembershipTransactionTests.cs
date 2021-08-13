using DragonSpark.Application.Runtime;
using DragonSpark.Compose;
using DragonSpark.Model.Sequences.Collections;
using FluentAssertions;
using JetBrains.Annotations;
using System;
using System.Linq;
using Xunit;

namespace DragonSpark.Application.Testing.Runtime
{
	public class MembershipTransactionTests
	{
		readonly static Guid[] Ids = {Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()};

		sealed class Subject
		{
			public Guid Id { get; set; }

			public string Message { [UsedImplicitly] get; set; } = default!;
		}

		sealed class Transactional : Transactional<Subject>
		{
			public static Transactional Default { get; } = new Transactional();

			Transactional() : base(new DelegatedEqualityComparer<Subject, Guid>(x => x.Id), _ => false) {}
		}

		[Fact]
		public void Verify()
		{
			var stored = Array.Empty<Subject>();

			var current = new[]
			{
				new Subject {Id = Ids[0], Message = "First"},
				new Subject {Id = Ids[1], Message = "Second"},
				new Subject {Id = Ids[2], Message = "Third"},
				new Subject {Id = Ids[3], Message = "Fourth"},
				new Subject {Id = Ids[4], Message = "Fifth"}
			};

			using var transactions = Transactional.Default.Get((stored, current));
			transactions.Add.Length.Should().Be(5);
			transactions.Update.Length.Should().Be(0);
			transactions.Delete.Length.Should().Be(0);

			var sut      = MembershipTransaction<Subject>.Default;
			var instance = stored.ToList();
			sut.Execute(instance, transactions);
			instance.Should().HaveCount(5).And.Subject.Should().BeEquivalentTo(current);

		}

		[Fact]
		public void VerifyAdded()
		{
			var stored = new[]
			{
				new Subject {Id = Ids[2], Message = "Third"},
				new Subject {Id = Ids[4], Message = "Fifth"}
			};

			var current = new[]
			{
				new Subject {Id = Ids[0], Message = "First"},
				new Subject {Id = Ids[1], Message = "Second"},
				new Subject {Id = Ids[2], Message = "Third"},
				new Subject {Id = Ids[3], Message = "Fourth"},
				new Subject {Id = Ids[4], Message = "Fifth"}
			};

			using var transactions = Transactional.Default.Get((stored, current));
			transactions.Add.Length.Should().Be(3);
			transactions.Update.Length.Should().Be(0);
			transactions.Delete.Length.Should().Be(0);

			var sut      = MembershipTransaction<Subject>.Default;
			var instance = stored.ToList();
			sut.Execute(instance, transactions);
			instance.Should().HaveCount(5).And.Subject.Should().BeEquivalentTo(current);
		}

		[Fact]
		public void VerifyRemove()
		{
			var stored = new[]
			{
				new Subject {Id = Ids[0], Message = "First"},
				new Subject {Id = Ids[1], Message = "Second"},
				new Subject {Id = Ids[2], Message = "Third"},
				new Subject {Id = Ids[3], Message = "Fourth"},
				new Subject {Id = Ids[4], Message = "Fifth"}
			};

			var current = new[]
			{
				new Subject {Id = Ids[0], Message = "First"},
				new Subject {Id = Ids[2], Message = "Third"},
				new Subject {Id = Ids[4], Message = "Fifth"}
			};

			using var transactions = Transactional.Default.Get((stored, current));
			transactions.Add.Length.Should().Be(0);
			transactions.Update.Length.Should().Be(0);
			transactions.Delete.Length.Should().Be(2);

			var sut      = MembershipTransaction<Subject>.Default;
			var instance = stored.ToList();
			sut.Execute(instance, transactions);
			instance.Should().HaveCount(3).And.Subject.Should().BeEquivalentTo(current);
		}

	}
}