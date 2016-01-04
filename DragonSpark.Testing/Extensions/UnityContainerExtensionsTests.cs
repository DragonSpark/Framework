using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Objects;
using Microsoft.Practices.Unity;
using Ploeh.AutoFixture.Xunit2;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Extensions
{
	public class UnityContainerExtensionsTests
	{
		[Theory, AutoData]
		public void TryResolve( UnityContainer sut )
		{
			var messages = sut.DetermineLogger().AsTo<RecordingMessageLogger, IEnumerable<Message>>( logger => logger.Recorder.Messages );
			Assert.False( messages.Any() );

			var item = sut.TryResolve<IInterface>();
			Assert.Null( item );

			Assert.True( messages.Any() );
		} 
	}
}