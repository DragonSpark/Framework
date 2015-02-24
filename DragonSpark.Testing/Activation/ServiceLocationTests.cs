using DragonSpark.Activation;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.TestObjects;
using Microsoft.Practices.ServiceLocation;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;
using ServiceLocator = DragonSpark.Testing.Framework.ServiceLocator;

namespace DragonSpark.Testing.Activation
{
	public class ServiceLocationTests
	{
		[Fact]
		public void IsAvailable()
		{
			Assert.False( ServiceLocation.IsAvailable() );

			Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider( () => new ServiceLocator( new Fixture() ) );

			var isAvailable = ServiceLocation.IsAvailable();
			Assert.True( isAvailable );
		}

		/*[Fact]
		public void Assign()
		{
			var assigned = false;
			ServiceLocation.Assigned += ( sender, args ) => assigned = true;
			ServiceLocation.Assign( new ServiceLocator( new Fixture() ) );
			Assert.True( assigned );
		}*/

		[Theory, AutoDataCustomization, AssignServiceLocation]
		public void RegisterGeneric()
		{
			ServiceLocation.Register<IInterface, Class>();

			var located = ServiceLocation.Locate<IInterface>();
			Assert.IsType<Class>( located );
		}

		[Theory, AutoDataCustomization, AssignServiceLocation]
		public void Register()
		{
			ServiceLocation.Register( typeof(IInterface), typeof(Class) );

			var located = ServiceLocation.Locate<IInterface>();
			Assert.IsType<Class>( located );
		}

		[Theory, AutoDataCustomization, AssignServiceLocation]
		void RegisterInstanceGeneric( Class instance )
		{
			ServiceLocation.Register<IInterface>( instance );

			var located = ServiceLocation.Locate<IInterface>();
			Assert.IsType<Class>( located );
			Assert.Equal( instance, located );
		}

		[Theory, Framework.AutoDataCustomization, AssignServiceLocation]
		void RegisterInstance( Class instance )
		{
			ServiceLocation.Register( typeof(IInterface), instance );

			var located = ServiceLocation.Locate<IInterface>();
			Assert.IsType<Class>( located );
			Assert.Equal( instance, located );
		}

		[Theory, Framework.AutoDataCustomization, AssignServiceLocation]
		void RegisterFactory( Class instance )
		{
			ServiceLocation.Register<IInterface>( () => instance );

			var located = ServiceLocation.Locate<IInterface>();
			Assert.IsType<Class>( located );
			Assert.Equal( instance, located );
		}

		[Theory, Framework.AutoDataCustomization, AssignServiceLocation]
		public void With( [Frozen]ClassWithParameter instance )
		{
			var item = ServiceLocation.With<ClassWithParameter, object>( x => x.Parameter );
			Assert.Equal( instance.Parameter, item );

			Assert.Null( ServiceLocation.With<IInterface, object>( x => x ) );
		}

		[Fact]
		public void WithDefault()
		{
			var item = ServiceLocation.With<ClassWithParameter, object>( x => x.Parameter );
			Assert.Null( item );
		}

		[Theory, AutoMockData, AssignServiceLocation]
		public void Registry( IServiceRegistry registry,  Mock<IServiceRegistry> sut )
		{
			registry.Register( typeof(IServiceRegistry), sut.Object );

			ServiceLocation.Register<IInterface, Class>();

			sut.Verify( x => x.Register( typeof(IInterface), typeof(Class), null ) );
		}
	}
}