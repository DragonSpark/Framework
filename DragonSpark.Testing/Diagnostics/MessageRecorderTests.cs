using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Testing.Framework.Setup;
using Xunit;

namespace DragonSpark.Testing.Diagnostics
{
	public class MessageRecorderTests
	{
		[Theory, AutoData]
		public void Message( RecordingMessageLogger sut, string message, Priority priority )
		{
			sut.Information( message, priority );

			var item = sut.Messages.Only();
			Assert.NotNull( item );

			Assert.Equal( nameof(MessageLoggerExtensions.Information), item.Category );
			Assert.Equal( priority, item.Priority );
		}

		[Theory, AutoData]
		public void Fatal( RecordingMessageLogger sut, string message, FatalApplicationException error )
		{
			sut.Fatal( message, error );

			var item = sut.Messages.Only();
			Assert.NotNull( item );

			Assert.Equal( nameof(Fatal), item.Category );
			Assert.Contains( error.GetType().Name, item.Text );
		}
	}
}