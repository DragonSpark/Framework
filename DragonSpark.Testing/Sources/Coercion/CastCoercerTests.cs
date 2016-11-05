using DragonSpark.Diagnostics;
using DragonSpark.Sources.Coercion;
using Serilog;
using System;
using Xunit;

namespace DragonSpark.Testing.Sources.Coercion
{
	public class CastCoercerTests
	{
		[Fact]
		public void Verify()
		{
			Assert.Throws<InvalidOperationException>( () => CastCoercer<LoggerConfiguration, LoggerFactory.LoggerConfiguration>.Default.Get( new LoggerConfiguration() ) );
			Assert.Throws<InvalidOperationException>( () => LoggerFactory.Factory.Implementation.Get( new LoggerConfiguration() ) );
		}
	}
}