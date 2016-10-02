using System.Linq;
using DragonSpark.Diagnostics;
using DragonSpark.Diagnostics.Configurations;
using DragonSpark.Sources;
using Ploeh.AutoFixture.Xunit2;
using Serilog.Core;
using Serilog.Events;
using Xunit;
using Logger = DragonSpark.Diagnostics.Logger;

namespace DragonSpark.Testing.Diagnostics
{
	public class LoggingControllerTests
	{
		[Fact]
		public void BasicContext()
		{
			var level = MinimumLevelConfiguration.Default;
			var controller = LoggingController.Default;

			var first = controller.Get();
			Assert.Same( first, controller.Get() );

			Assert.Equal( LogEventLevel.Information, first.MinimumLevel );

			const LogEventLevel assigned = LogEventLevel.Debug;
			level.Assign( assigned );
			controller.Assign( Factory.GlobalCache( () => new LoggingLevelSwitch( MinimumLevelConfiguration.Default.Get() ) ) );

			var second = controller.Get();
			Assert.NotSame( first, second );
			Assert.Same( second, controller.Get() );

			Assert.Equal( assigned, second.MinimumLevel );
		}

		[Theory, AutoData]
		void VerifyHistory( object context, string message )
		{
			var history = LoggingHistory.Default.Get();
			Assert.Empty( history.Events );
			Assert.Same( history, LoggingHistory.Default.Get() );

			var logger = Logger.Default.Get( context );
			Assert.Empty( history.Events );
			logger.Information( "Hello World! {Message}", message );
			Assert.Single( history.Events, item => item.RenderMessage().Contains( message ) );

			logger.Debug( "Hello World! {Message}", message );
			Assert.Single( history.Events );
			LoggingController.Default.Get().MinimumLevel = LogEventLevel.Debug;

			logger.Debug( "Hello World! {Message}", message );
			Assert.Equal( 2, history.Events.Count() );
		}
	}
}
