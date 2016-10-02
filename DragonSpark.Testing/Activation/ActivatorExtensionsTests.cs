using DragonSpark.Activation;
using DragonSpark.Testing.Objects;
using Ploeh.AutoFixture.Xunit2;
using Xunit;
using Constructor = DragonSpark.Activation.Constructor;

namespace DragonSpark.Testing.Activation
{
	public class ActivatorExtensionsTests
	{
		[Theory, AutoData]
		public void ActivateMany()
		{
			var sut = Constructor.Default;
			var items = sut.ActivateMany<IInterface>( new[] { typeof(Class), typeof(AnotherClass), typeof(YetAnotherClass) } );
			Assert.Collection( items, x => Assert.IsType<Class>( x ), x => Assert.IsType<AnotherClass>( x ), x => Assert.IsType<YetAnotherClass>( x ) );
		} 
	}
}