using DragonSpark.Diagnostics.Exceptions;
using System;
using Xunit;

namespace DragonSpark.Testing.Diagnostics.Exceptions
{
	public class BackoffRetryTimeTests
	{
		[Fact]
		public void Coverage()
		{
			const int seconds = 3;
			Assert.Equal( TimeSpan.FromSeconds( (int)Math.Pow( seconds, 2 ) ), BackOffRetryTime.Default.Get( seconds ) );
		}
	}
}