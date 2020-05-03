using DragonSpark.Compose;
using FluentAssertions;
using System.Collections.Immutable;
using Xunit;

namespace DragonSpark.Testing.Runtime
{
	public sealed class IsAssignedTests
	{
		[Fact]
		public void VerifyImmutableArrays()
		{
			Is.Assigned<ImmutableArray<object>>().Get().Get(default).Should().BeFalse();
			Is.Assigned<ImmutableArray<object>>().Get().Get(ImmutableArray<object>.Empty).Should().BeTrue();
			Is.Assigned<ImmutableArray<object>>().Get().Get(ImmutableArray.Create(new object())).Should().BeTrue();
		}

		[Fact]
		public void VerifyReferences()
		{
			Is.Assigned<object>().Get().Get(new object()).Should().BeTrue();
			Is.Assigned<object>().Get().Get(null!).Should().BeFalse();
		}

		[Fact]
		public void VerifyUnassignedValues()
		{
			Is.Assigned<int?>().Get().Get(1).Should().BeTrue();
			Is.Assigned<int?>().Get().Get(0).Should().BeTrue();
			Is.Assigned<int?>().Get().Get(null).Should().BeFalse();
		}

		[Fact]
		public void VerifyValues()
		{
			Is.Assigned<int>().Get().Get(1).Should().BeTrue();
			Is.Assigned<int>().Get().Get(0).Should().BeFalse();
		}
	}
}