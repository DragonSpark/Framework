using DragonSpark.Activation;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Parameters;
using DragonSpark.Testing.Framework.Setup;
using DragonSpark.Testing.TestObjects;
using Xunit;
using Activator = DragonSpark.Testing.TestObjects.Activator;

namespace DragonSpark.Testing.Activation
{
	[Freeze( typeof(IActivator), typeof(Activator) )]
	public class CurrentActivatorTests
	{
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

		[Theory, Test, SetupAutoData]
		public void CreateInstance( [Registered]IActivator activator )
		{
			Assert.Same( DragonSpark.Activation.Activator.Current, activator );
			Assert.NotSame( SystemActivator.Instance, activator );
			Assert.IsType<Activator>( activator );
			var instance = activator.Activate<IObject>( typeof(Object) );
			Assert.IsType<Object>( instance );

			Assert.Equal( "DefaultActivation", instance.Name );
		}

		[Theory, Test, SetupAutoData]
		public void CreateNamedInstance( [Registered]IActivator activator, string name )
		{
			Assert.Same( DragonSpark.Activation.Activator.Current, activator );
			Assert.NotSame( SystemActivator.Instance, activator );

			var instance = activator.Activate<IObject>( typeof(Object), name );
			Assert.IsType<Object>( instance );

			Assert.Equal( name, instance.Name );
		}

		[Theory, Test, SetupAutoData]
		public void CreateItem( [Registered]IActivator activator )
		{
			var parameters = new object[] { typeof(Object), "This is Some Name." };
			Assert.Same( DragonSpark.Activation.Activator.Current, activator );
			Assert.NotSame( SystemActivator.Instance, activator );
			var instance = activator.Construct<Item>( parameters );
			Assert.NotNull( instance );

			Assert.Equal( parameters, instance.Parameters );
		}
	}
}