using DragonSpark.Compose;
using DragonSpark.Reflection.Collections;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace DragonSpark.Testing.Reflection.Collections
{
	public sealed class CollectionInnerTypeTests
	{
		[Fact]
		public void Verify()
		{
			CollectionInnerType.Default.Get(A.Type<ICollection<DateTimeOffset>>())
			                   .Should()
			                   .Be(A.Type<DateTimeOffset>());
		}
	}
}