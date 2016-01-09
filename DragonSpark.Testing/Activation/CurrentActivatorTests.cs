using DragonSpark.Activation;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Objects;
using Xunit;
using Xunit.Abstractions;

namespace DragonSpark.Testing.Activation
{
	public class CurrentActivatorTests : Tests
	{
		public CurrentActivatorTests( ITestOutputHelper output ) : base( output )
		{}

		[Fact]
		public void Default()
		{
			var activator = DragonSpark.Activation.Activator.GetCurrent();
			Assert.Same( SystemActivator.Instance, activator );
			var instance = activator.Activate<IInterface>( typeof(Class) );
			Assert.IsType<Class>( instance );
		}

		[Theory, Ploeh.AutoFixture.Xunit2.AutoData]
		public void DefaultCreate( string parameter )
		{
			var activator = DragonSpark.Activation.Activator.GetCurrent();
			Assert.Same( SystemActivator.Instance, activator );
			
			var instance = activator.Construct<ClassWithParameter>( parameter );
			Assert.NotNull( instance );
			Assert.Equal( parameter, instance.Parameter );
		}
	}
}