using DragonSpark.Activation;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.TestObjects;
using Microsoft.Practices.ServiceLocation;
using Moq;
using Ploeh.AutoFixture.Xunit2;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;
using ServiceLocation = DragonSpark.Activation.ServiceLocation;
using ServiceLocator = DragonSpark.Activation.IoC.ServiceLocator;

namespace DragonSpark.Testing.Activation
{
	public class DefaultServicesTests : Tests
	{
		public DefaultServicesTests( ITestOutputHelper output ) : base( output )
		{}

		[Theory, Test, SetupAutoData]
		void RegisterInstanceGeneric( ServiceLocation sut, Class instance )
		{
			Assert.IsType<ServiceLocation>( Services.Location );

			Assert.Same( Services.Location, sut );

			sut.Register<IInterface>( instance );

			var located = sut.Locate<IInterface>();
			Assert.IsType<Class>( located );
			Assert.Equal( instance, located );
		}

		[Fact]
		public void IsAvailable()
		{
			var sut = Services.Location;

			Assert.Same( sut, ServiceLocation.Instance );

			Assert.False( sut.IsAvailable );

			sut.Assign( new ServiceLocator().Prepared( MethodBase.GetCurrentMethod() ) );

			var isAvailable = sut.IsAvailable;
			Assert.True( isAvailable );

			sut.Assign( null );
			Assert.False( sut.IsAvailable );
		}

		[Theory, Test, SetupAutoData]
		public void RegisterGeneric( ServiceLocation sut )
		{
			sut.Register<IInterface, Class>();

			var located = sut.Locate<IInterface>();
			Assert.IsType<Class>( located );
		}

		[Theory, Test, SetupAutoData]
		public void Register( ServiceLocation sut )
		{
			sut.Register( typeof(IInterface), typeof(Class) );

			var located = sut.Locate<IInterface>();
			Assert.IsType<Class>( located );
		}

		[Theory, Test, SetupAutoData]
		void RegisterInstance( ServiceLocation sut, Class instance )
		{
			sut.Register( typeof(IInterface), instance );

			var located = sut.Locate<IInterface>();
			Assert.IsType<Class>( located );
			Assert.Equal( instance, located );
		}

		[Theory, Test, SetupAutoData]
		void RegisterFactory( ServiceLocation sut, Class instance )
		{
			sut.Register<IInterface>( () => instance );

			var located = sut.Locate<IInterface>();
			Assert.IsType<Class>( located );
			Assert.Equal( instance, located );
		}

		[Theory, Test, SetupAutoData]
		public void With( ServiceLocation sut, IServiceLocator locator, [Frozen, Registered]ClassWithParameter instance )
		{
			Assert.Same( Services.Location, sut );
			Assert.Same( sut.Locator, locator );
			var item = sut.With<ClassWithParameter, object>( x => x.Parameter );
			Assert.Equal( instance.Parameter, item );

			Assert.Null( sut.With<IInterface, object>( x => x ) );
		}

		[Theory, Test, SetupAutoData]
		public void WithDefault( ServiceLocation sut )
		{
			var item = sut.With<ClassWithParameter, bool>( x => x.Parameter != null );
			Assert.True( item );
		}

		[Theory, Test, SetupAutoData]
		public void RegisterWithRegistry( ServiceLocation location, Mock<IServiceRegistry> sut )
		{
			Assert.Same( ServiceLocation.Instance, location );

			location.Assign( new ServiceLocator().Prepared( MethodBase.GetCurrentMethod() ) );

			location.Register( typeof(IServiceRegistry), sut.Object );

			location.Register<IInterface, Class>();

			sut.Verify( x => x.Register( typeof(IInterface), typeof(Class), null ) );
		}
	}
}