using DragonSpark.Activation;
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
using ServiceLocator = DragonSpark.Activation.IoC.ServiceLocator;

namespace DragonSpark.Testing.Activation.IoC
{
	public class ServiceLocatorTests
	{
		[Theory, Test, Framework.AutoData]
		void GetInstance( [Modest, Frozen]ServiceLocator sut, [Frozen, Registered]ILogger logger )
		{
			Assert.NotSame( Services.Location.Locator, sut );

			var before = sut.GetInstance<IInterface>();
			Assert.Null( before );

			var mock = Mock.Get( logger );

			mock.Verify( x => x.Warning( @"Specified type is not registered: ""DragonSpark.Testing.TestObjects.IInterface"" with build name ""<None>"".", Priority.High ) );

			sut.Register<IInterface, Class>();

			var after = sut.GetInstance<IInterface>();
			Assert.IsType<Class>( after );

			var broken = sut.GetInstance<ClassWithBrokenConstructor>();
			Assert.Null( broken );

			mock.Verify( x => x.Warning( It.Is<string>( y => y.StartsWith( @"Could not resolve type ""ClassWithBrokenConstructor"" with build name ""<None>"".  Details: Microsoft.Practices.Unity.ResolutionFailedException: Resolution of the dependency failed, type = ""DragonSpark.Testing.TestObjects.ClassWithBrokenConstructor"", name = ""(none)""." ) ), Priority.High ) );
		}

		[Theory, Test, Framework.AutoData]
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

		[Theory, Test, Framework.AutoData]
		void Container( [Frozen] DragonSpark.Activation.IoC.ServiceLocator sut )
		{
			sut.Dispose();

			Assert.Throws<ObjectDisposedException>( () => sut.Container );
		}

		[Register( typeof(IActivator), typeof(Activator))]
		[Theory, Test, Framework.AutoData]
		void Create( IActivator sut, ILogger logger, string message, int number, Class @item )
		{
			Assert.IsType<Activator>( sut );

			var created = sut.Construct<ClassWithManyParameters>( number, message, item );
			Assert.Equal( message, created.String );
			Assert.Equal( number, created.Integer );
			Assert.Equal( item, created.Class );

			Assert.NotNull( sut.Construct<ClassCreatedFromDefault>( "Create from default" ) );
		}
		
		[Theory, Framework.AutoData( typeof(Customizations.Assigned) )]
		void Register( IUnityContainer container, [Frozen]ServiceLocator sut )
		{
			Assert.False( container.IsRegistered<IInterface>() );
			sut.Register<IInterface,Class>();
			Assert.True( container.IsRegistered<IInterface>() );
			Assert.IsType<Class>( container.Resolve<IInterface>() );
		}

		[Theory, Framework.AutoData( typeof(Customizations.Assigned) )]
		void RegisterInstance( IUnityContainer container, [Frozen]ServiceLocator sut )
		{
			Assert.False( container.IsRegistered<IInterface>() );
			var instance = new Class();
			sut.Register<IInterface>( instance );
			Assert.True( container.IsRegistered<IInterface>() );
			Assert.IsType<Class>( container.Resolve<IInterface>() );
			Assert.Equal( instance, container.Resolve<IInterface>() );
		}

		[Theory, Framework.AutoData( typeof(Customizations.Assigned) )]
		void RegisterFactory( IUnityContainer container, [Frozen]ServiceLocator sut )
		{
			Assert.False( container.IsRegistered<IInterface>() );
			sut.RegisterFactory( typeof(IInterface), () => new Class() );
			Assert.True( container.IsRegistered<IInterface>() );
			Assert.IsType<Class>( container.Resolve<IInterface>() );
		}

		[Theory, Framework.AutoData( typeof(Customizations.Assigned) )]
		void BuildUp( [Frozen( Matching.ImplementedInterfaces )]ServiceLocator sut )
		{
			sut.Register<IDefaultValueProvider, DefaultValueProvider>();

			var item = new ClassWithDefaultProperties();

			Assert.Null( item.String );

			Assert.True( item.BuildUpOnce() );
			Assert.Equal( "Hello World", item.String );
			Assert.False( item.BuildUpOnce() );
		}
	}
}