using System.Threading;
using DragonSpark.Activation;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.TestObjects;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;
using Xunit;
using ServiceLocator = DragonSpark.Testing.Framework.ServiceLocator;

namespace DragonSpark.Testing.Activation
{
	public class ServiceLocationTests
	{
		[Fact]
		public void IsAvailable()
		{
			Assert.False( Services.IsAvailable() );

			Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider( () => new ServiceLocator( new Fixture() ) );

			var isAvailable = Services.IsAvailable();
			Assert.True( isAvailable );
		}

		/*[Fact]
		public void Assign()
		{
			var assigned = false;
			Services.Assigned += ( sender, args ) => assigned = true;
			Services.Assign( new ServiceLocator( new Fixture() ) );
			Assert.True( assigned );
		}*/

		[Theory, AutoDataCustomization, AssignServiceLocation]
		public void RegisterGeneric()
		{
			// ExecutionContext.

			Services.Register<IInterface, Class>();

			var located = Services.Locate<IInterface>();
			Assert.IsType<Class>( located );
		}

		[Theory, AutoDataCustomization, AssignServiceLocation]
		public void Register()
		{
			Services.Register( typeof(IInterface), typeof(Class) );

			var located = Services.Locate<IInterface>();
			Assert.IsType<Class>( located );
		}

		[Theory, AutoDataCustomization, AssignServiceLocation]
		void RegisterInstanceGeneric( Class instance )
		{
			Services.Register<IInterface>( instance );

			var located = Services.Locate<IInterface>();
			Assert.IsType<Class>( located );
			Assert.Equal( instance, located );
		}

		[Theory, Framework.AutoDataCustomization, AssignServiceLocation]
		void RegisterInstance( Class instance )
		{
			Services.Register( typeof(IInterface), instance );

			var located = Services.Locate<IInterface>();
			Assert.IsType<Class>( located );
			Assert.Equal( instance, located );
		}

		[Theory, Framework.AutoDataCustomization, AssignServiceLocation]
		void RegisterFactory( Class instance )
		{
			Services.Register<IInterface>( () => instance );

			var located = Services.Locate<IInterface>();
			Assert.IsType<Class>( located );
			Assert.Equal( instance, located );
		}

		[Theory, Framework.AutoDataCustomization, AssignServiceLocation]
		public void With( [Frozen]ClassWithParameter instance )
		{
			var item = Services.With<ClassWithParameter, object>( x => x.Parameter );
			Assert.Equal( instance.Parameter, item );

			Assert.Null( Services.With<IInterface, object>( x => x ) );
		}

		[Fact]
		public void WithDefault()
		{
			var item = Services.With<ClassWithParameter, object>( x => x.Parameter );
			Assert.Null( item );
		}

		[Theory, AutoMockData, AssignServiceLocation]
		public void Registry( IServiceRegistry registry,  Mock<IServiceRegistry> sut )
		{
			registry.Register( typeof(IServiceRegistry), sut.Object );

			Services.Register<IInterface, Class>();

			sut.Verify( x => x.Register( typeof(IInterface), typeof(Class), null ) );
		}
	}
}