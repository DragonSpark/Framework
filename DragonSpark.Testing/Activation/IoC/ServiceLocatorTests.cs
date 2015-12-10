using DragonSpark.Activation;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.TestObjects;
using Microsoft.Practices.Unity;
using Moq;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using DragonSpark.Testing.Framework.Parameters;
using DragonSpark.Testing.Framework.Setup;
using Xunit;
using ServiceLocator = DragonSpark.Activation.IoC.ServiceLocator;

namespace DragonSpark.Testing.Activation.IoC
{
	public class ServiceLocatorTests
	{
		[Theory, Test, SetupAutoData]
		void GetInstance( [Modest, Frozen]ServiceLocator sut, [Frozen, Registered]Mock<ILogger> logger )
		{
			Assert.NotSame( Services.Location.Locator, sut );
			Assert.Same( sut.Get<ILogger>(), logger.Object );

			var before = sut.GetInstance<IInterface>();
			Assert.Null( before );

			logger.Verify( x => x.Warning( @"Specified type is not registered: ""DragonSpark.Testing.TestObjects.IInterface"" with build name ""<None>"".", Priority.High ) );

			sut.Register<IInterface, Class>();

			var after = sut.GetInstance<IInterface>();
			Assert.IsType<Class>( after );

			var broken = sut.GetInstance<ClassWithBrokenConstructor>();
			Assert.Null( broken );

			logger.Verify( x => x.Exception( @"Could not resolve type ""ClassWithBrokenConstructor"" with build name ""<None>"".", It.IsAny<ResolutionFailedException>() ) );
		}

		[Theory, Test, SetupAutoData]
		void GetAllInstances( [Modest, Frozen] ServiceLocator sut )
		{
			sut.Register<IInterface, Class>( "First" );
			sut.Register<IInterface, Derived>( "Second" );

			var count = sut.Container.Registrations.Count( x => x.RegisteredType == typeof(IInterface) );
			Assert.Equal( 2, count );

			var items = sut.GetAllInstances<IInterface>();
			Assert.Equal( 2, items.Count() );

			var classes = new[]{ new Class() };
			sut.Register<IEnumerable<IInterface>>( classes );

			var updated = sut.GetAllInstances<IInterface>();
			Assert.Equal( 3, updated.Count() );

			Assert.Contains( classes.Single(), updated );
		}

		[Theory, Test, SetupAutoData]
		void Container( [Modest, Frozen] ServiceLocator sut )
		{
			sut.Dispose();

			Assert.Throws<ObjectDisposedException>( () => sut.Container );
		}

		[Theory, Test, SetupAutoData]
		void Create( IActivator sut, string message, int number, Class @item )
		{
			Assert.IsType<CompositeActivator>( sut );

			var created = sut.Construct<ClassWithManyParameters>( number, message, item );
			Assert.Equal( message, created.String );
			Assert.Equal( number, created.Integer );
			Assert.Equal( item, created.Class );

			var systemMessage = "Create from system";
			var systemCreated = sut.Construct<ClassCreatedFromDefault>( systemMessage );
			Assert.NotNull( systemCreated );
			Assert.Equal( systemMessage, systemCreated.Message );
		}
		
		[Theory, Test, SetupAutoData]
		void Register( IUnityContainer container, [Frozen]ServiceLocator sut )
		{
			Assert.False( container.IsRegistered<IInterface>() );
			sut.Register<IInterface,Class>();
			Assert.True( container.IsRegistered<IInterface>() );
			Assert.IsType<Class>( container.Resolve<IInterface>() );
		}

		[Theory, Test, SetupAutoData]
		void RegisterInstance( IUnityContainer container, [Frozen]ServiceLocator sut )
		{
			Assert.False( container.IsRegistered<IInterface>() );
			var instance = new Class();
			sut.Register<IInterface>( instance );
			Assert.True( container.IsRegistered<IInterface>() );
			Assert.IsType<Class>( container.Resolve<IInterface>() );
			Assert.Equal( instance, container.Resolve<IInterface>() );
		}

		[Theory, Test, SetupAutoData]
		void RegisterFactory( IUnityContainer container, [Frozen]ServiceLocator sut )
		{
			Assert.False( container.IsRegistered<IInterface>() );
			sut.RegisterFactory( typeof(IInterface), () => new Class() );
			Assert.True( container.IsRegistered<IInterface>() );
			Assert.IsType<Class>( container.Resolve<IInterface>() );
		}

		[Theory, Test, SetupAutoData]
		void BuildUp( ServiceLocator sut )
		{
			// sut.Register<IDefaultValueProvider, DefaultValueProvider>();

			var item = new ClassWithDefaultProperties();

			Assert.Null( item.String );

			item.BuildUp();
			// Assert.True( item.BuildUp() );
			Assert.Equal( "Hello World", item.String );
			// Assert.False( item.BuildUp() );
		}


		[Theory, Test, SetupAutoData]
		void Dispose( [Greedy, Frozen, Assigned] ServiceLocator sut )
		{
			Assert.IsAssignableFrom<ServiceLocation>( Services.Location );

			Assert.Same( sut, Services.Location.Locator );

			var item = DragonSpark.Activation.Activator.Current.Activate<IInterface>( typeof(Class) );
			Assert.NotNull( item );

			var disposable = new Disposable();
			
			sut.Register( disposable );

			Assert.False( disposable.Disposed );
			
			Assert.Same( Services.Location.Locator, sut );

			sut.Dispose();

			Assert.NotSame( Services.Location.Locator, sut );

			Assert.True( disposable.Disposed );
		}
	}
}