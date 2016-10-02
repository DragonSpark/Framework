using DragonSpark.Commands;
using DragonSpark.Diagnostics;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using Xunit;
using Defaults = DragonSpark.Sources.Defaults;

namespace DragonSpark.Testing.Sources
{
	public class TimedDelegatedSourceTests
	{
		[Fact]
		public void VerifySource()
		{
			var source = new DelegatedSource<int>( () => 6776 ).Timed();
			var history = LoggingHistory.Default.Get();
			Assert.Empty( history.Events );
			var result = source();
			Assert.Equal( 6776, result );
			var item = Assert.Single( history.Events );
			var text = item.MessageTemplate.Text;
			Assert.Contains( Defaults.TimerTemplate, text );
		}

		[Fact]
		public void VerifyParameterizedSource()
		{
			var source = new DelegatedParameterizedSource<object, int>( o => 6776 ).Timed();
			var history = LoggingHistory.Default.Get();
			Assert.Empty( history.Events );
			var result = source( new object() );
			Assert.Equal( 6776, result );
			var item = Assert.Single( history.Events );
			var text = item.MessageTemplate.Text;
			Assert.Contains( Defaults.ParameterizedTimerTemplate, text );
		}

		[Fact]
		public void VerifyCommand()
		{
			var action = new DelegatedCommand<int>( parameter => {} ).Timed();
			var history = LoggingHistory.Default.Get();
			Assert.Empty( history.Events );
			action( 6776 );
			var item = Assert.Single( history.Events );
			var text = item.MessageTemplate.Text;
			Assert.Contains( Defaults.ParameterizedTimerTemplate, text );
		}
	}
}
