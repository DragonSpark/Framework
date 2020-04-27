using DragonSpark.Reflection.Types;
using FluentAssertions;
using System;
using Xunit;

namespace DragonSpark.Testing.Reflection.Types
{
	public class IsOfTests
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