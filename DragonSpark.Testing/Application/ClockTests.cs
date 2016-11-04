using DragonSpark.Application;
using System;
using Xunit;

namespace DragonSpark.Testing.Application
{
	public class ClockTests
	{
		[Fact]
		public void Coverage()
		{
			var now = DateTimeOffset.Now;
			Assert.True( Clock.DefaultImplementation.Implementation.Get() >= now );
		}
	}
}