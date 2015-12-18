using DragonSpark.Activation;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Setup;
using DragonSpark.Testing.TestObjects;
using Xunit;

namespace DragonSpark.Testing.Activation
{
	public class ActivatorTests
	{
		[Theory, Test, SetupAutoData]
		public void ActivateMany( SystemActivator sut )
		{
			var items = sut.ActivateMany<IInterface>( new[] { typeof(Class), typeof(AnotherClass), typeof(YetAnotherClass) } );
			Assert.Collection( items, x => Assert.IsType<Class>( x ), x => Assert.IsType<AnotherClass>( x ), x => Assert.IsType<YetAnotherClass>( x ) );
		} 
	}
}