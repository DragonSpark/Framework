using DragonSpark.Compose;
using DragonSpark.Model.Sequences.Collections;
using FluentAssertions;
using System;
using Xunit;

namespace DragonSpark.Application.Testing
{
	public sealed class TransactionalTests
	{
		readonly static Guid[] Ids = {Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()};

		sealed class Subject
		{
			public Guid Id { get; set; }

			public string Message { get; set; } = default!;
		}

		sealed class Transactional : Transactional<Subject>
		{
			public static Transactional Default { get; } = new Transactional();

			Transactional() : base(new DelegatedEqualityComparer<Subject, Guid>(x => x.Id),
			                       x => x.Item1.Message != x.Item2.Message) {}
		}

		[Fact]
		public void Verify()
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
				new Subject {Id = Ids[1], Message = "Second"},
				new Subject {Id = Ids[2], Message = "Third"},
				new Subject {Id = Ids[3], Message = "Fourth"},
				new Subject {Id = Ids[4], Message = "Fifth"}
			};

			var transactions = Transactional.Default.Get((stored, current));
			transactions.Add.Open().Should().BeEmpty();
			transactions.Update.Open().Should().BeEmpty();
			transactions.Delete.Open().Should().BeEmpty();
		}

		[Fact]
		public void VerifyAdd()
		{
			var stored = new[]
			{
				new Subject {Id = Ids[1], Message = "Second"},
				new Subject {Id = Ids[2], Message = "Third"},
				new Subject {Id = Ids[3], Message = "Fourth"},
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

			var transactions = Transactional.Default.Get((stored, current));
			transactions.Add.Open().Only().Should().BeSameAs(current[0]);
			transactions.Update.Open().Should().BeEmpty();
			transactions.Delete.Open().Should().BeEmpty();
		}

		[Fact]
		public void VerifyAddUpdateDelete()
		{
			var stored = new[]
			{
				new Subject {Id = Ids[0], Message = "First"},
				new Subject {Id = Ids[1], Message = "Second"},
				new Subject {Id = Ids[2], Message = "Third"},
				new Subject {Id = Ids[4], Message = "Fifth"}
			};

			var current = new[]
			{
				new Subject {Id = Ids[1], Message = "Second"},
				new Subject {Id = Ids[2], Message = "Third - Modified"},
				new Subject {Id = Ids[3], Message = "Fourth"},
				new Subject {Id = Ids[4], Message = "Fifth"}
			};

			var transactions = Transactional.Default.Get((stored, current));
			transactions.Add.Open().Only().Should().BeSameAs(current[2]);
			var (subject, source) = transactions.Update.Open().Only();
			subject.Should().BeSameAs(stored[2]);
			source.Should().BeSameAs(current[1]);
			transactions.Delete.Open().Only().Should().BeSameAs(stored[0]);
		}

		[Fact]
		public void VerifyDelete()
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
				new Subject {Id = Ids[1], Message = "Second"},
				new Subject {Id = Ids[3], Message = "Fourth"},
				new Subject {Id = Ids[4], Message = "Fifth"}
			};

			var transactions = Transactional.Default.Get((stored, current));
			transactions.Add.Open().Should().BeEmpty();
			transactions.Update.Open().Should().BeEmpty();
			transactions.Delete.Open().Only().Should().BeSameAs(stored[2]);
		}

		[Fact]
		public void VerifyModified()
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
				new Subject {Id = Ids[1], Message = "Second"},
				new Subject {Id = Ids[2], Message = "Third"},
				new Subject {Id = Ids[3], Message = "Fourth - Modified"},
				new Subject {Id = Ids[4], Message = "Fifth"}
			};

			var transactions = Transactional.Default.Get((stored, current));
			transactions.Add.Open().Should().BeEmpty();
			var (subject, source) = transactions.Update.Open().Only();
			subject.Should().BeSameAs(stored[3]);
			source.Should().BeSameAs(current[3]);
			transactions.Delete.Open().Should().BeEmpty();
		}
	}
}