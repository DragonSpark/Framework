using DragonSpark.Activation;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.TestObjects;
using Xunit;
using Activator = DragonSpark.Testing.TestObjects.Activator;

namespace DragonSpark.Testing.Activation
{
	public class ActivatorTests
	{
		[Fact]
		public void Default()
		{
			var instance = DragonSpark.Activation.Activator.CreateInstance<IInterface>( typeof(Class) );
			Assert.IsType<Class>( instance );
		}

		[Theory, Ploeh.AutoFixture.Xunit2.AutoData]
		public void DefaultCreate( string parameter )
		{
			var instance = DragonSpark.Activation.Activator.Create<ClassWithParameter>( parameter );
			
			Assert.NotNull( parameter );
			Assert.Equal( parameter, instance.Parameter );
		}

		[Freeze( typeof(IActivator), typeof(Activator) )]
		[Theory, AutoDataCustomization, Services]
		public void CreateInstance()
		{
			var instance = DragonSpark.Activation.Activator.CreateInstance<IObject>( typeof(Object) );
			Assert.IsType<Object>( instance );

			Assert.Equal( "DefaultActivation", instance.Name );
		}

		[Freeze( typeof(IActivator), typeof(Activator) )]
		[Theory, AutoDataCustomization, Services]
		public void CreateNamedInstance( string name )
		{
			var instance = DragonSpark.Activation.Activator.CreateNamedInstance<IObject>( typeof(Object), name );
			Assert.IsType<Object>( instance );

			Assert.Equal( name, instance.Name );
		}

		[Freeze( typeof(IActivator), typeof(Activator) )]
		[Theory, AutoDataCustomization, Services]
		public void CreateItem()
		{
			var parameters = new object[] { typeof(Object), "This is Some Name." };
			var instance = DragonSpark.Activation.Activator.Create<Item>( parameters );
			Assert.NotNull( instance );

			Assert.Equal( parameters, instance.Parameters );
		}
	}
}