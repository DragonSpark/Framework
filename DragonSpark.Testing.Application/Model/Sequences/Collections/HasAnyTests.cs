using FluentAssertions;
using DragonSpark.Model.Sequences.Collections;
using DragonSpark.Runtime;
using Xunit;

namespace DragonSpark.Testing.Application.Model.Sequences.Collections
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