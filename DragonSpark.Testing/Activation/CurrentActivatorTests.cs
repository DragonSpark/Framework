using DragonSpark.Activation;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Parameters;
using DragonSpark.Testing.TestObjects;
using Xunit;
using Xunit.Abstractions;
using Activator = DragonSpark.Testing.TestObjects.Activator;

namespace DragonSpark.Testing.Activation
{
	[Freeze( typeof(IActivator), typeof(Activator) )]
	public class CurrentActivatorTests : Tests
	{
		public CurrentActivatorTests( ITestOutputHelper output ) : base( output )
		{}

		[Fact]
		public void Default()
		{
			var activator = DragonSpark.Activation.Activator.Current;
			Assert.Same( SystemActivator.Instance, activator );
			var instance = activator.Activate<IInterface>( typeof(Class) );
			Assert.IsType<Class>( instance );
		}

		[Theory, Ploeh.AutoFixture.Xunit2.AutoData]
		public void DefaultCreate( string parameter )
		{
			var activator = DragonSpark.Activation.Activator.Current;
			Assert.Same( SystemActivator.Instance, activator );
			
			var instance = activator.Construct<ClassWithParameter>( parameter );
			Assert.NotNull( instance );
			Assert.Equal( parameter, instance.Parameter );
		}
	}
}