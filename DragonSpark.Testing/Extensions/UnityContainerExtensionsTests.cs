using System.Linq;
using DragonSpark.Activation.IoC;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Objects;
using Microsoft.Practices.Unity;
using Xunit;

namespace DragonSpark.Testing.Extensions
{
	public class UnityContainerExtensionsTests
	{
		[Theory, Framework.Setup.AutoData]
		public void TryResolve( [Factory]UnityContainer sut )
		{
			var logger = sut.Resolve<RecordingMessageLogger>();
			var initial = logger.Messages.Count();
			Assert.NotEmpty( logger.Messages );

			Assert.False( sut.IsRegistered<ISingletonLocator>() );

			var item = sut.TryResolve<IInterface>();
			Assert.Null( item );

			Assert.True( sut.IsRegistered<ISingletonLocator>() );

			var count = logger.Messages.Count();
			Assert.True( count > initial );

			/*var register = new RecordingMessageLogger();
			sut.RegisterInstance<IMessageLogger>( register );

			Assert.Empty( logger.Messages );
			Assert.NotEmpty( register.Messages );

			var after = register.Messages.Count();
			Assert.Equal( count + 2, after );

			Assert.Null( sut.TryResolve<IInterface>() );
			Assert.Equal( after + 2, register.Messages.Count() );
			Assert.Empty( logger.Messages );*/
		}
	}
}