using DragonSpark.Model.Selection.Stores;
using FluentAssertions;
using JetBrains.Annotations;
using System;
using Xunit;

namespace DragonSpark.Application.Testing.Runtime;

public sealed class ThrottlingTests
{
	[Fact]
	public void Verify()
	{
		var table = new Table<Key, object>();

		var reference = new object();
		var first     = new Key("Hello", reference);
		var second    = new Key("Hello", reference);

		table.Execute((first, reference));

		table.Condition.Get(first).Should().BeTrue();
		table.Condition.Get(second).Should().BeTrue();
		table.Get(second).Should().BeSameAs(reference);
	}

	[Fact]
	public void VerifyConcurrent()
	{
		var table = new ConcurrentTable<Key, object>();

		var reference = new object();
		var first     = new Key("Hello", reference);
		var second    = new Key("Hello", reference);

		table.Execute((first, reference));

		table.Condition.Get(first).Should().BeTrue();
		table.Condition.Get(second).Should().BeTrue();
		table.Get(second).Should().BeSameAs(reference);
	}

	[Fact]
	public void VerifyDelegate()
	{
		var table = new Table<DelegateKey, object>();

		var first     = new DelegateKey("Hello", VerifyDelegate);
		var second    = new DelegateKey("Hello", VerifyDelegate);

		var reference = new object();
		table.Execute((first, reference));

		table.Condition.Get(first).Should().BeTrue();
		table.Condition.Get(second).Should().BeTrue();
		table.Get(second).Should().BeSameAs(reference);
	}

	readonly record struct Key([UsedImplicitly] string Name, [UsedImplicitly] object Reference);

	readonly record struct DelegateKey([UsedImplicitly] string Name, [UsedImplicitly] Delegate Reference);
}