using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework.Setup;
using System;
using Moq;
using Xunit;

namespace DragonSpark.Testing.Diagnostics
{
	public class MessageRecorderTests
	{
		[Theory, ConfiguredMoqAutoData]
		public void Message( RecordingMessageLogger sut, string message, Priority priority )
		{
			sut.Information( message, priority );

			var item = sut.Recorder.Messages.Only();
			Assert.NotNull( item );

			Assert.Equal( nameof(sut.Information), item.Category );
			Assert.Equal( priority, item.Priority );
		}

		[Theory, ConfiguredMoqAutoData]
		public void Fatal( RecordingMessageLogger sut, string message, Exception error )
		{
			sut.Fatal( message, error );

			var item = sut.Recorder.Messages.Only();
			Assert.NotNull( item );

			Assert.Equal( nameof(sut.Fatal), item.Category );
			Assert.Contains( error.GetType().Name, item.Text );
		}
	}
}