using Ploeh.AutoFixture.Xunit2;
using System;
using Xunit;
using ServiceLocator = DragonSpark.Activation.IoC.ServiceLocator;

namespace DragonSpark.Testing.Activation.IoC
{
	public class ServiceLocatorTests
	{
		[Theory, AutoData]
		void Container( [Modest, Frozen] ServiceLocator sut )
		{
			sut.Dispose();

			Assert.Throws<ObjectDisposedException>( () => sut.Container );
		}
	}
}