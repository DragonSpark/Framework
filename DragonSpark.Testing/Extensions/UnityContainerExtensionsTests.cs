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
			var logger = sut.Extend().Resolve<RecordingMessageLogger>();
			var initial = logger.Messages.Count();
			Assert.NotEmpty( logger.Messages );

			var item = sut.TryResolve<IInterface>();
			Assert.Null( item );

			var count = logger.Messages.Count();
			Assert.Equal( initial + 2, count );

			var register = new RecordingMessageLogger();
			sut.RegisterInstance<IMessageLogger>( register );

			Assert.Empty( logger.Messages );
			Assert.NotEmpty( register.Messages );

			var after = register.Messages.Count();
			Assert.Equal( count + 2, after );

			Assert.Null( sut.TryResolve<IInterface>() );
			Assert.Equal( after + 2, register.Messages.Count() );
			Assert.Empty( logger.Messages );
		}
	}
}