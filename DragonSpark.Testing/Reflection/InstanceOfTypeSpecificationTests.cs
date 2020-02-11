using DragonSpark.Reflection.Types;
using FluentAssertions;
using System;
using Xunit;

namespace DragonSpark.Testing.Reflection
{
	public class InstanceOfTypeSpecificationTests
	{
		[Fact]
		public void Verify()
		{
			IsOf<int>.Default.Get(6776)
			         .Should()
			         .BeTrue();

			IsOf<int>.Default.Get(DateTime.Now)
			         .Should()
			         .BeFalse();
		}
	}
}