using DragonSpark.Model;
using DragonSpark.Model.Sequences.Collections;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Model.Sequences.Collections
{
	public class HasAnyTests
	{
		[Fact]
		public void Has() => HasAny.Default.Get(new[] {new object()})
		                           .Should()
		                           .BeTrue();

		[Fact]
		public void HasNot() => HasAny.Default.Get(Empty<object>.Array)
		                              .Should()
		                              .BeFalse();
	}
}