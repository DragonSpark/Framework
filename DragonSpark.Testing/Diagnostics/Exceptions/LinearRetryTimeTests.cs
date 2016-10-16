using DragonSpark.Diagnostics.Exceptions;
using System;
using Xunit;

namespace DragonSpark.Testing.Diagnostics.Exceptions
{
	public class LinearRetryTimeTests
	{
		[Fact]
		public void Coverage()
		{
			const int seconds = 3;
			Assert.Equal( TimeSpan.FromSeconds( seconds ), LinearRetryTime.Default.Get( seconds ) );
		}
	}
}