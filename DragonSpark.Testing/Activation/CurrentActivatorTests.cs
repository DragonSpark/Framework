using DragonSpark.Activation;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.TestObjects;
using Xunit;
using Activator = DragonSpark.Testing.TestObjects.Activator;

namespace DragonSpark.Testing.Activation
{
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
			Assert.NotNull( parameter );
			Assert.Equal( parameter, instance.Parameter );
		}

		[Freeze( typeof(IActivator), typeof(Activator) )]
		[Theory, AutoData( typeof(Customizations.Assigned) )]
		public void CreateInstance()
		{
			var activator = DragonSpark.Activation.Activator.Current;
			Assert.NotSame( SystemActivator.Instance, activator );
			
			var instance = activator.Activate<IObject>( typeof(Object) );
			Assert.IsType<Object>( instance );

			Assert.Equal( "DefaultActivation", instance.Name );
		}

		[Freeze( typeof(IActivator), typeof(Activator) )]
		[Theory, AutoData( typeof(Customizations.Assigned) )]
		public void CreateNamedInstance( string name )
		{
			var activator = DragonSpark.Activation.Activator.Current;
			Assert.NotSame( SystemActivator.Instance, activator );

			var instance = activator.Activate<IObject>( typeof(Object), name );
			Assert.IsType<Object>( instance );

			Assert.Equal( name, instance.Name );
		}

		[Freeze( typeof(IActivator), typeof(Activator) )]
		[Theory, AutoData( typeof(Customizations.Assigned) )]
		public void CreateItem()
		{
			var parameters = new object[] { typeof(Object), "This is Some Name." };
			var activator = DragonSpark.Activation.Activator.Current;
			Assert.NotSame( SystemActivator.Instance, activator );
			var instance = activator.Construct<Item>( parameters );
			Assert.NotNull( instance );

			Assert.Equal( parameters, instance.Parameters );
		}
	}
}