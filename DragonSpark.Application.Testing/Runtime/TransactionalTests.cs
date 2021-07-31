using DragonSpark.Application.Runtime;
using DragonSpark.Compose;
using DragonSpark.Model.Sequences.Collections;
using FluentAssertions;
using System;
using Xunit;

namespace DragonSpark.Application.Testing.Runtime
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

			using var transactions = Transactional.Default.Get((stored, current));
			transactions.Add.Length.Should().Be(0);
			transactions.Update.Length.Should().Be(0);
			transactions.Delete.Length.Should().Be(0);
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

			using var transactions = Transactional.Default.Get((stored, current));
			transactions.Add.AsSpan().ToArray().Only().Should().BeSameAs(current[0]);
			transactions.Update.AsSpan().ToArray().Should().BeEmpty();
			transactions.Delete.AsSpan().ToArray().Should().BeEmpty();
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

			using var transactions = Transactional.Default.Get((stored, current));
			transactions.Add.AsSpan().ToArray().Only().Should().BeSameAs(current[2]);
			var (subject, source) = transactions.Update.AsSpan().ToArray().Only();
			subject.Should().BeSameAs(stored[2]);
			source.Should().BeSameAs(current[1]);
			transactions.Delete.AsSpan().ToArray().Only().Should().BeSameAs(stored[0]);
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

			using var transactions = Transactional.Default.Get((stored, current));
			transactions.Add.AsSpan().ToArray().Should().BeEmpty();
			transactions.Update.AsSpan().ToArray().Should().BeEmpty();
			transactions.Delete.AsSpan().ToArray().Only().Should().BeSameAs(stored[2]);
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

			using var transactions = Transactional.Default.Get((stored, current));
			transactions.Add.AsSpan().ToArray().Should().BeEmpty();
			var (subject, source) = transactions.Update.AsSpan().ToArray().Only();
			subject.Should().BeSameAs(stored[3]);
			source.Should().BeSameAs(current[3]);
			transactions.Delete.AsSpan().ToArray().Should().BeEmpty();
		}
	}
}