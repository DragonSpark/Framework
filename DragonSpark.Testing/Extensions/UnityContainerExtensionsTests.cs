using System.Linq;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Testing.Objects;
using Microsoft.Practices.Unity;
using Xunit;

namespace DragonSpark.Testing.Extensions
{
	public class UnityContainerExtensionsTests
	{
		[Theory, Framework.Setup.AutoData]
		public void TryResolve( UnityContainer sut )
		{
			var logger = sut.Extend().Logger;
			Assert.Single( logger.Messages );

			var item = sut.TryResolve<IInterface>();
			Assert.Null( item );

			var count = logger.Messages.Count();
			Assert.True( count > 1 );

			var register = new RecordingMessageLogger();
			sut.RegisterInstance<IMessageLogger>( register );

			Assert.Empty( logger.Messages );
			Assert.NotEmpty( register.Messages );
			Assert.Equal( count, register.Messages.Count() );

			Assert.Null( sut.TryResolve<IInterface>() );
			Assert.True( register.Messages.Count() > count );
			Assert.Empty( logger.Messages );
		}
	}
}