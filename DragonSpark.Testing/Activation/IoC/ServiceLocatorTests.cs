using Ploeh.AutoFixture.Xunit2;
using System;
using PostSharp.Patterns.Model;
using Xunit;
using ServiceLocator = DragonSpark.Activation.IoC.ServiceLocator;

namespace DragonSpark.Testing.Activation.IoC
{
	public class ServiceLocatorTests
	{
		[Theory, AutoData]
		void Container( [Modest, Frozen] ServiceLocator sut )
		{
			sut.QueryInterface<IDisposable>().Dispose();

			Assert.Throws<ObjectDisposedException>( () => sut.Container );
		}
	}
}