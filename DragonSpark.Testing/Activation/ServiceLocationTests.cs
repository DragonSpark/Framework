using DragonSpark.Activation;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Testing.TestObjects;
using DragonSpark.Testing.TestObjects;
using Microsoft.Practices.ServiceLocation;
using Moq;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace DragonSpark.Testing.Activation
{
	public class ServiceLocationTests
	{
		public ServiceLocationTests()
		{
			ServiceLocation.Assign( null );
		}

		[Fact]
		public void IsAvailable()
		{
			Assert.False( ServiceLocation.IsAvailable() );

			ServiceLocation.Assign( new ServiceLocationCustomization() );

			var isAvailable = ServiceLocation.IsAvailable();
			Assert.True( isAvailable );
		}

		[Fact]
		public void Assign()
		{
			var assigned = false;
			ServiceLocation.Assigned += ( sender, args ) => assigned = true;
			ServiceLocation.Assign( new ServiceLocationCustomization() );
			Assert.True( assigned );
		}

		[Theory, Framework.AutoData, AssignServiceLocation]
		public void RegisterGeneric()
		{
			ServiceLocation.Register<IInterface, Class>();

			var located = ServiceLocation.Locate<IInterface>();
			Assert.IsType<Class>( located );
		}

		[Theory, Framework.AutoData, AssignServiceLocation]
		public void Register()
		{
			ServiceLocation.Register( typeof(IInterface), typeof(Class) );

			var located = ServiceLocation.Locate<IInterface>();
			Assert.IsType<Class>( located );
		}

		[Theory, Framework.AutoData, AssignServiceLocation]
		void RegisterInstanceGeneric( Class instance )
		{
			ServiceLocation.Register<IInterface>( instance );

			var located = ServiceLocation.Locate<IInterface>();
			Assert.IsType<Class>( located );
			Assert.Equal( instance, located );
		}

		[Theory, Framework.AutoData, AssignServiceLocation]
		void RegisterInstance( Class instance )
		{
			ServiceLocation.Register( typeof(IInterface), instance );

			var located = ServiceLocation.Locate<IInterface>();
			Assert.IsType<Class>( located );
			Assert.Equal( instance, located );
		}

		[Theory, Framework.AutoData, AssignServiceLocation]
		void RegisterFactory( Class instance )
		{
			ServiceLocation.Register<IInterface>( () => instance );

			var located = ServiceLocation.Locate<IInterface>();
			Assert.IsType<Class>( located );
			Assert.Equal( instance, located );
		}

		[Theory, Framework.AutoData, AssignServiceLocation]
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

		[Theory, Framework.AutoData, AssignServiceLocation]
		public void Registry( [Frozen]IServiceLocator locator, [FreezeObject]Mock<IServiceRegistry> registry )
		{
			ServiceLocation.Register<IInterface, Class>();
			registry.Verify( x => x.Register( typeof(IInterface), typeof(Class) ) );
		}
	}
}