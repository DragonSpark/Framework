﻿using DragonSpark.Compose;
using FluentAssertions;
using System.Collections.Immutable;
using Xunit;

namespace DragonSpark.Testing.Application.Runtime
{
	public sealed class IsAssignedTests
	{
		[Fact]
		void VerifyImmutableArrays()
		{
			Is.Assigned<ImmutableArray<object>>().Get(default).Should().BeFalse();
			Is.Assigned<ImmutableArray<object>>().Get(ImmutableArray<object>.Empty).Should().BeTrue();
			Is.Assigned<ImmutableArray<object>>().Get(ImmutableArray.Create(new object())).Should().BeTrue();
		}

		[Fact]
		void VerifyReferences()
		{
			Is.Assigned<object>().Get(new object()).Should().BeTrue();
			Is.Assigned<object>().Get(null).Should().BeFalse();
		}

		[Fact]
		void VerifyUnassignedValues()
		{
			Is.Assigned<int?>().Get(1).Should().BeTrue();
			Is.Assigned<int?>().Get(0).Should().BeTrue();
			Is.Assigned<int?>().Get(null).Should().BeFalse();
		}

		[Fact]
		void VerifyValues()
		{
			Is.Assigned<int>().Get(1).Should().BeTrue();
			Is.Assigned<int>().Get(0).Should().BeFalse();
		}
	}
}