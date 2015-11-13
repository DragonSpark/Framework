using DragonSpark.Activation;
using DragonSpark.Activation.IoC;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.TestObjects;
using Dynamitey;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Linq;
using DragonSpark.Runtime;
using Xunit;
using Xunit.Abstractions;
using Activator = DragonSpark.Activation.IoC.Activator;
using ServiceLocation = DragonSpark.Activation.ServiceLocation;
using ServiceLocator = DragonSpark.Testing.Framework.ServiceLocator;

namespace DragonSpark.Testing.Activation
{
	public class DefaultServicesTests : IDisposable
	{
		public DefaultServicesTests( ITestOutputHelper output )
		{
			AmbientValues.Register( GetType(), output );
		}

		[Theory, AutoDataCustomization, DefaultServices, EnableOutput]
		void RegisterInstanceGeneric( ServiceLocation sut, Class instance )
		{
			Assert.IsType<ServiceLocation>( Services.Location );

			Assert.Same( sut, Services.Location );

			sut.Register<IInterface>( instance );

			var located = sut.Locate<IInterface>();
			Assert.IsType<Class>( located );
			Assert.Equal( instance, located );
		}

		[Fact]
		public void IsAvailable()
		{
			var sut = new ServiceLocation();

			Assert.False( sut.IsAvailable );

			sut.Assign( new ServiceLocator( new Fixture() ) );

			var isAvailable = sut.IsAvailable;
			Assert.True( isAvailable );

			sut.Assign( null );
			Assert.False( sut.IsAvailable );
		}

		[Theory, AutoDataCustomization, DefaultServices]
		public void RegisterGeneric( ServiceLocation sut )
		{
			sut.Register<IInterface, Class>();

			var located = sut.Locate<IInterface>();
			Assert.IsType<Class>( located );
		}

		[Theory, AutoDataCustomization, DefaultServices]
		public void Register( ServiceLocation sut )
		{
			sut.Register( typeof(IInterface), typeof(Class) );

			var located = sut.Locate<IInterface>();
			Assert.IsType<Class>( located );
		}

		[Theory, AutoDataCustomization, DefaultServices]
		void RegisterInstance( ServiceLocation sut, Class instance )
		{
			sut.Register( typeof(IInterface), instance );

			var located = sut.Locate<IInterface>();
			Assert.IsType<Class>( located );
			Assert.Equal( instance, located );
		}

		[Theory, AutoDataCustomization, DefaultServices]
		void RegisterFactory( ServiceLocation sut, Class instance )
		{
			sut.Register<IInterface>( () => instance );

			var located = sut.Locate<IInterface>();
			Assert.IsType<Class>( located );
			Assert.Equal( instance, located );
		}

		[Theory, AutoDataCustomization, DefaultServices]
		public void With( ServiceLocation sut, IServiceLocator locator, [Frozen]ClassWithParameter instance )
		{
			var item = sut.With<ClassWithParameter, object>( x => x.Parameter );
			Assert.Equal( instance.Parameter, item );

			Assert.Null( sut.With<IInterface, object>( x => x ) );
		}

		[Theory, AutoDataCustomization, DefaultServices]
		public void WithDefault( ServiceLocation sut )
		{
			var item = sut.With<ClassWithParameter, bool>( x => x.Parameter != null );
			Assert.True( item );
		}

		[Freeze( typeof(IUnityContainer), typeof(UnityContainer) )]
		[Freeze( typeof(IActivator), typeof(Activator) )]
		[ContainerExtensionFactory( typeof(IoCExtension) )]
		[Theory, AutoDataCustomization, DefaultServices]
		void Dispose( [Greedy, Frozen, Assign] DragonSpark.Activation.IoC.ServiceLocator sut, IoCExtension extension )
		{
			Assert.IsAssignableFrom<ServiceLocation>( Services.Location );

			Assert.Same( sut, Services.Location.Locator );

			var child = sut.Container.CreateChildContainer();
			child.Registrations.First( x => x.RegisteredType == typeof(IUnityContainer) ).LifetimeManager.RemoveValue();

			Assert.Equal( sut.Container, child.Resolve<IUnityContainer>() );

			var item = DragonSpark.Activation.Activator.CreateInstance<IInterface>( typeof(Class) );
			Assert.NotNull( item );

			var disposable = new Disposable();
			
			sut.Register( disposable );

			Assert.Equal( 1, extension.Children.Count );

			Assert.False( disposable.Disposed );
			
			Assert.True( Microsoft.Practices.ServiceLocation.ServiceLocator.IsLocationProviderSet );

			sut.Dispose();

			var temp = Dynamic.InvokeGet( child, "lifetimeContainer" );
			Assert.Null( temp );

			Assert.Equal( 0, extension.Children.Count );

			Assert.False( Microsoft.Practices.ServiceLocation.ServiceLocator.IsLocationProviderSet );

			Assert.True( disposable.Disposed );
		}

		[Theory, AutoMockData]
		public void Registry( ServiceLocation host, Mock<IServiceRegistry> sut )
		{
			host.Assign( new ServiceLocator( new Fixture() ) );

			host.Register( typeof(IServiceRegistry), sut.Object );

			host.Register<IInterface, Class>();

			sut.Verify( x => x.Register( typeof(IInterface), typeof(Class), null ) );
		}

		public void Dispose()
		{
			AmbientValues.Clear( GetType() );
		}
	}
}