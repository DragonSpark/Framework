using FluentAssertions;
using System;
using System.Diagnostics;
using Xunit;

namespace DragonSpark.Testing.Diagnostics.Logging.Configuration
{
	// ReSharper disable once TestFileNameWarning
	public class EnhancedExceptionStackTraceConfigurationTests
	{
		[Fact]
		public void Verify()
		{
			try
			{
				throw new InvalidOperationException("hello!");
			}
			catch (Exception e)
			{
				e.ToString().Should().NotBe(e.Demystify().ToString());
			}
		}
	}
}