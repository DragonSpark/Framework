using System.Collections.Immutable;
using FluentAssertions;
using DragonSpark.Runtime;
using Xunit;

namespace DragonSpark.Testing.Application.Runtime
{
	public sealed class IsAssignedTests
	{
		[Fact]
		void VerifyImmutableArrays()
		{
			IsAssigned<ImmutableArray<object>>.Default.Get(default).Should().BeFalse();
			IsAssigned<ImmutableArray<object>>.Default.Get(ImmutableArray<object>.Empty).Should().BeTrue();
			IsAssigned<ImmutableArray<object>>.Default.Get(ImmutableArray.Create(new object())).Should().BeTrue();
		}

		[Fact]
		void VerifyReferences()
		{
			IsAssigned<object>.Default.Get(new object()).Should().BeTrue();
			IsAssigned<object>.Default.Get(null).Should().BeFalse();
		}

		[Fact]
		void VerifyUnassignedValues()
		{
			IsAssigned<int?>.Default.Get(1).Should().BeTrue();
			IsAssigned<int?>.Default.Get(0).Should().BeTrue();
			IsAssigned<int?>.Default.Get(null).Should().BeFalse();
		}

		[Fact]
		void VerifyValues()
		{
			IsAssigned<int>.Default.Get(1).Should().BeTrue();
			IsAssigned<int>.Default.Get(0).Should().BeFalse();
		}
	}
}