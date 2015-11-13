using DragonSpark.Activation;
using DragonSpark.Activation.IoC;
using DragonSpark.ComponentModel;
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
using Xunit;
using Activator = DragonSpark.Activation.IoC.Activator;

namespace DragonSpark.Testing.Activation.IoC
{
	[Freeze( typeof(IUnityContainer), typeof(UnityContainer) )]
	[Freeze( typeof(IActivator), typeof(Activator) )]
	[ContainerExtensionFactory( typeof(IoCExtension) )]
	public class ServiceLocatorTests
	{
		[Theory, Services, AutoMockData]
		void GetInstance( [Frozen]ILogger logger, [Frozen]DragonSpark.Activation.IoC.ServiceLocator sut )
		{
			var before = sut.GetInstance<IInterface>();
			Assert.Null( before);

			var mock = Mock.Get( logger );

			mock.Verify( x => x.Warning( @"Specified type is not registered: ""DragonSpark.Testing.TestObjects.IInterface"" with build name ""<None>"".", Priority.High ) );

			sut.Register<IInterface, Class>();

			var after = sut.GetInstance<IInterface>();
			Assert.IsType<Class>( after );

			var broken = sut.GetInstance<ClassWithBrokenConstructor>();
			Assert.Null( broken );

			mock.Verify( x => x.Warning( It.Is<string>( y => y.StartsWith( @"Could not resolve type ""ClassWithBrokenConstructor"" with build name ""<None>"".  Details: Microsoft.Practices.Unity.ResolutionFailedException: Resolution of the dependency failed, type = ""DragonSpark.Testing.TestObjects.ClassWithBrokenConstructor"", name = ""(none)""." ) ), Priority.High ) );
		}

		[Theory, AutoDataCustomization]
		void GetAllInstances( [Frozen] DragonSpark.Activation.IoC.ServiceLocator sut )
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

		[Theory, AutoDataCustomization]
		void Container( [Frozen] DragonSpark.Activation.IoC.ServiceLocator sut )
		{
			sut.Dispose();

			Assert.Throws<ObjectDisposedException>( () => sut.Container );
		}

		[Theory, AutoDataCustomization, Services]
		void Create(  [Greedy, Frozen] DragonSpark.Activation.IoC.ServiceLocator locator, IActivator sut, string message, int number, Class @item )
		{
			locator.Register<IDefaultValueProvider, DefaultValueProvider>();

			var created = sut.Create<ClassWithManyParameters>( number, message, item );
			Assert.Equal( message, created.String );
			Assert.Equal( number, created.Integer );
			Assert.Equal( item, created.Class );

			Assert.NotNull( sut.Create<ClassCreatedFromDefault>( "Create from default" ) );
		}
		
		[Theory, AutoDataCustomization, Services]
		void Register( IUnityContainer container, [Greedy, Frozen] DragonSpark.Activation.IoC.ServiceLocator sut )
		{
			Assert.False( container.IsRegistered<IInterface>() );
			sut.Register<IInterface,Class>();
			Assert.True( container.IsRegistered<IInterface>() );
			Assert.IsType<Class>( container.Resolve<IInterface>() );
		}

		[Theory, AutoDataCustomization, Services]
		void RegisterInstance( IUnityContainer container, [Greedy, Frozen] DragonSpark.Activation.IoC.ServiceLocator sut )
		{
			Assert.False( container.IsRegistered<IInterface>() );
			var instance = new Class();
			sut.Register<IInterface>( instance );
			Assert.True( container.IsRegistered<IInterface>() );
			Assert.IsType<Class>( container.Resolve<IInterface>() );
			Assert.Equal( instance, container.Resolve<IInterface>() );
		}

		[Theory, AutoDataCustomization, Services]
		void RegisterFactory( IUnityContainer container, [Greedy, Frozen] DragonSpark.Activation.IoC.ServiceLocator sut )
		{
			Assert.False( container.IsRegistered<IInterface>() );
			sut.RegisterFactory( typeof(IInterface), () => new Class() );
			Assert.True( container.IsRegistered<IInterface>() );
			Assert.IsType<Class>( container.Resolve<IInterface>() );
		}

		[Theory, AutoDataCustomization, Services]
		void BuildUp( [Greedy, Frozen( Matching.ImplementedInterfaces )] DragonSpark.Activation.IoC.ServiceLocator sut, IoCExtension extension )
		{
			sut.Register<IDefaultValueProvider, DefaultValueProvider>();
			Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider( () => sut );
			var item = new ClassWithDefaultProperties();

			Assert.Null( item.String );

			Assert.True( item.BuildUpOnce() );
			Assert.Equal( "Hello World", item.String );
			Assert.False( item.BuildUpOnce() );
		}
	}
}