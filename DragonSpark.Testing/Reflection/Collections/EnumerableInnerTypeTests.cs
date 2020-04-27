using DragonSpark.Compose;
using DragonSpark.Reflection.Collections;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace DragonSpark.Testing.Reflection.Collections
{
	public class EnumerableInnerTypeTests
	{
		[Fact]
		public void Array()
		{
			EnumerableInnerType.Default.Get(typeof(int[]))
			                   .Should()
			                   .Be<int>();
		}

		[Fact]
		public void Enumerable()
		{
			EnumerableInnerType.Default.Get(typeof(IEnumerable<DateTimeOffset>))
			                   .Should()
			                   .Be<DateTimeOffset>();
		}
	}
}