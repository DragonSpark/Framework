using FluentAssertions;
using System;
using System.Diagnostics;
using Xunit;

namespace DragonSpark.Testing.Diagnostics.Logging.Configuration
{
	public class EnhancedExceptionStackTraceConfigurationTests
	{
		[Fact]
		void Verify()
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