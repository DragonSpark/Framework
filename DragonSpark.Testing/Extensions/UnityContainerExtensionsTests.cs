using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Testing.Objects;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Extensions
{
	public class UnityContainerExtensionsTests
	{
		[Theory, Framework.Setup.AutoData]
		public void TryResolve( UnityContainer sut )
		{
			var messages = sut.DetermineLogger().AsTo<RecordingMessageLogger, IEnumerable<Message>>( logger => logger.Recorder.Messages );
			Assert.False( messages.Any() );

			var item = sut.TryResolve<IInterface>();
			Assert.Null( item );

			var register = new RecordingMessageLogger();
			sut.RegisterInstance<IMessageLogger>( register );

			Assert.NotEmpty( messages );

			var count = messages.Count();

			Assert.Empty( register.Recorder.Messages );
			Assert.Null( sut.TryResolve<IInterface>() );
			Assert.NotEmpty( register.Recorder.Messages );
			Assert.Equal( count, messages.Count() );
		} 
	}
}