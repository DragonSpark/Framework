using DragonSpark.Model.Selection.Stores;
using FluentAssertions;
using JetBrains.Annotations;
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

	readonly record struct Key([UsedImplicitly] string Name, [UsedImplicitly] object Reference);
}