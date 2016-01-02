using DragonSpark.Activation;
using DragonSpark.Activation.IoC;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Setup;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Parameters;
using DragonSpark.Testing.Framework.Setup;
using DragonSpark.Testing.Objects;
using DragonSpark.TypeSystem;
using DragonSpark.Windows.Testing.TestObjects;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Moq;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;
using Activator = DragonSpark.Windows.Testing.TestObjects.Activator;
using Attribute = DragonSpark.Testing.Objects.Attribute;
using Object = DragonSpark.Testing.Objects.Object;
using ServiceLocator = DragonSpark.Activation.IoC.ServiceLocator;

namespace DragonSpark.Windows.Testing.Setup
{
	/// <summary>
	/// This file can be seen as a bucket for all the testing done around setup.  It also can be seen as a huge learning bucket for xUnit and AutoFixture.  This does not contain best practices.  Always be learning. :)
	/// </summary>
	[AssignExecution]
	public class SetupTests : Tests
	{
		public SetupTests( ITestOutputHelper output ) : base( output )
		{}

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		public void CoreLocation( IServiceLocator sut )
		{
			Assert.True( Microsoft.Practices.ServiceLocation.ServiceLocator.IsLocationProviderSet );
			Assert.Same( sut, Microsoft.Practices.ServiceLocation.ServiceLocator.Current );
		}

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) ) ]
		public void MockAsExpected( [Located(false)]ISetup sut )
		{
			Assert.NotNull( Mock.Get( sut ) );
		}

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		public void SetupRegistered( ISetup sut )
		{
			Assert.IsType<DefaultSetup>( sut );
		}

		[DragonSpark.Testing.Framework.Register( typeof(IActivator), typeof(Activator))]
		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		public void CreateInstance( [Registered]IActivator activator )
		{
			Assert.Same( DragonSpark.Activation.Activator.Current, activator );
			Assert.NotSame( SystemActivator.Instance, activator );
			Assert.IsType<Activator>( activator );
			var instance = activator.Activate<IObject>( typeof(Object) );
			Assert.IsType<Object>( instance );

			Assert.Equal( "DefaultActivation", instance.Name );
		}

		[DragonSpark.Testing.Framework.Register( typeof( IActivator ), typeof( Activator ) )]
		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		public void CreateNamedInstance( [Registered]IActivator activator, string name )
		{
			Assert.Same( DragonSpark.Activation.Activator.Current, activator );
			Assert.NotSame( SystemActivator.Instance, activator );

			var instance = activator.Activate<IObject>( typeof(Object), name );
			Assert.IsType<Object>( instance );

			Assert.Equal( name, instance.Name );
		}

		[DragonSpark.Testing.Framework.Register( typeof( IActivator ), typeof( Activator ) )]
		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		public void CreateItem( [Registered]IActivator activator )
		{
			var parameters = new object[] { typeof(Object), "This is Some Name." };
			Assert.Same( DragonSpark.Activation.Activator.Current, activator );
			Assert.NotSame( SystemActivator.Instance, activator );
			var instance = activator.Construct<DragonSpark.Testing.Objects.Item>( parameters );
			Assert.NotNull( instance );

			Assert.Equal( parameters, instance.Parameters );
		}

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		void RegisterInstanceGeneric( [Located]ServiceLocation sut, Class instance )
		{
			Assert.IsType<ServiceLocation>( Services.Location );

			Assert.Same( Services.Location, sut );

			sut.Register<IInterface>( instance );

			var located = sut.Locate<IInterface>();
			Assert.IsType<Class>( located );
			Assert.Equal( instance, located );
		}

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		public void RegisterGeneric( ServiceLocation sut )
		{
			sut.Register<IInterface, Class>();

			var located = sut.Locate<IInterface>();
			Assert.IsType<Class>( located );
		}

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		public void RegisterLocation( ServiceLocation sut )
		{
			sut.Register( typeof( IInterface ), typeof( Class ) );

			var located = sut.Locate<IInterface>();
			Assert.IsType<Class>( located );
		}

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		void RegisterInstanceClass( ServiceLocation sut, Class instance )
		{
			sut.Register( typeof( IInterface ), instance );

			var located = sut.Locate<IInterface>();
			Assert.IsType<Class>( located );
			Assert.Equal( instance, located );
		}

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		void RegisterFactoryClass( ServiceLocation sut, Class instance )
		{
			sut.Register<IInterface>( () => instance );

			var located = sut.Locate<IInterface>();
			Assert.IsType<Class>( located );
			Assert.Equal( instance, located );
		}

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		public void With( [Located]ServiceLocation sut, IServiceLocator locator, [Frozen, Registered]ClassWithParameter instance )
		{
			Assert.Same( Services.Location, sut );
			Assert.Same( sut.Item, locator );
			var item = sut.With<ClassWithParameter, object>( x => x.Parameter );
			Assert.Equal( instance.Parameter, item );

			Assert.Null( sut.With<IInterface, object>( x => x ) );
		}

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		public void WithDefault( ServiceLocation sut )
		{
			var item = sut.With<ClassWithParameter, bool>( x => x.Parameter != null );
			Assert.True( item );
		}

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		public void RegisterWithRegistry( [Located]ServiceLocation location, Mock<IServiceRegistry> sut )
		{
			Assert.Same( ServiceLocation.Instance, location );

			location.Assign( DragonSpark.Activation.FactoryModel.Factory.Create<ServiceLocator>() );

			location.Register( typeof(IServiceRegistry), sut.Object );

			location.Register<IInterface, Class>();

			sut.Verify( x => x.Register( typeof( IInterface ), typeof( Class ), null ) );
		}

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		void Resolve( [Located]Interfaces sut )
		{
			Assert.NotNull( sut.Items.FirstOrDefaultOfType<Item>() );
			Assert.NotNull( sut.Items.FirstOrDefaultOfType<AnotherItem>() );
			Assert.NotNull( sut.Items.FirstOrDefaultOfType<YetAnotherItem>() );
		}

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		void GetInstance( [Factory, Frozen]ServiceLocator sut, [Frozen, Registered]Mock<IMessageLogger> logger )
		{
			Assert.NotSame( Services.Location.Item, sut );
			Assert.Same( sut.Get<IMessageLogger>(), logger.Object );

			var before = sut.GetInstance<IInterface>();
			Assert.Null( before );

			logger.Verify( x => x.Warning( $@"Specified type is not registered: ""{typeof(IInterface).FullName}"" with build name ""<None>"".", Priority.High ) );

			sut.Register<IInterface, Class>();

			var after = sut.GetInstance<IInterface>();
			Assert.IsType<Class>( after );

			var broken = sut.GetInstance<ClassWithBrokenConstructor>();
			Assert.Null( broken );

			logger.Verify( x => x.Exception( $@"Could not resolve type ""{typeof(ClassWithBrokenConstructor).Name}"" with build name ""<None>"".", It.IsAny<ResolutionFailedException>() ) );
		}

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		void GetAllInstancesLocator( [Modest, Factory] ServiceLocator sut )
		{
			sut.Register<IInterface, Class>( "First" );
			sut.Register<IInterface, Derived>( "Second" );

			var count = sut.Container.Registrations.Count( x => x.RegisteredType == typeof(IInterface) );
			Assert.Equal( 2, count );

			var items = sut.GetAllInstances<IInterface>();
			Assert.Equal( 2, items.Count() );

			var classes = new[]{ new Class() };
			sut.Register<IEnumerable<IInterface>>( classes );

			var updated = sut.GetAllInstances<IInterface>().Fixed();
			Assert.Equal( 3, updated.Length );

			Assert.Contains( classes.Single(), updated );
		}

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		void CreateActivator( IActivator sut, string message, int number, Class @item )
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

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		void Register( IUnityContainer container, [Frozen]ServiceLocator sut )
		{
			Assert.False( container.IsRegistered<IInterface>() );
			sut.Register<IInterface, Class>();
			Assert.True( container.IsRegistered<IInterface>() );
			Assert.IsType<Class>( container.Resolve<IInterface>() );
		}

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		void RegisterInstance( IUnityContainer container, [Frozen]ServiceLocator sut )
		{
			Assert.False( container.IsRegistered<IInterface>() );
			var instance = new Class();
			sut.Register<IInterface>( instance );
			Assert.True( container.IsRegistered<IInterface>() );
			Assert.IsType<Class>( container.Resolve<IInterface>() );
			Assert.Equal( instance, container.Resolve<IInterface>() );
		}

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		void RegisterFactory( IUnityContainer container, [Frozen]ServiceLocator sut )
		{
			Assert.False( container.IsRegistered<IInterface>() );
			sut.RegisterFactory( typeof( IInterface ), () => new Class() );
			Assert.True( container.IsRegistered<IInterface>() );
			Assert.IsType<Class>( container.Resolve<IInterface>() );
		}

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		void Dispose( [Factory, Frozen, Assigned] ServiceLocator sut )
		{
			var item = DragonSpark.Activation.Activator.Current.Activate<IInterface>( typeof(Class) );
			Assert.NotNull( item );

			var disposable = new Disposable();

			sut.Register( disposable );
			sut.Register( new ServiceLocationMonitor( Services.Location, sut ) );

			Assert.False( disposable.Disposed );

			Assert.Same( Services.Location.Item, sut );

			sut.Dispose();

			Assert.NotSame( Services.Location.Item, sut );

			Assert.True( disposable.Disposed );
		}

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		void RelayedPropertyAttribute()
		{
			var attribute = typeof(Relayed).GetProperty( "Property" ).GetAttribute<Attribute>();
			Assert.Equal( "This is a relayed property attribute.", attribute.PropertyName );
		}


		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		void RelayedAttribute()
		{
			var attribute = typeof(Relayed).GetAttribute<Attribute>();
			Assert.Equal( "This is a relayed class attribute.", attribute.PropertyName );
		}

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		public void Information( [Located( false ), Frozen]IMessageLogger messageLogger, string message )
		{
			messageLogger.Information( message );
			messageLogger.Information( message, Priority.High );

			Mock.Get( messageLogger ).Verify( x => x.Information( message, Priority.Normal ) );
			Mock.Get( messageLogger ).Verify( x => x.Information( message, Priority.High ) );
		}

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		public void Warning( [Located( false ), Frozen]IMessageLogger messageLogger, string message )
		{
			messageLogger.Warning( message );
			messageLogger.Warning( message, Priority.Low );

			Mock.Get( messageLogger ).Verify( x => x.Warning( message, Priority.High ) );
			Mock.Get( messageLogger ).Verify( x => x.Warning( message, Priority.Low ) );
		}

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		[DragonSpark.Testing.Framework.Register( typeof( IExceptionFormatter ), typeof( ExceptionFormatter ) )]
		public void Error( [Located( false ), Frozen]IMessageLogger messageLogger, IExceptionFormatter formatter, [Modest]InvalidOperationException error, string message )
		{
			// Assert.Same( logger, Log.Current );

			messageLogger.Exception( message, error );

			Mock.Get( messageLogger ).Verify( x => x.Exception( message, error ) );
		}

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		public void DefaultError( [Located( false ), Frozen]IMessageLogger messageLogger, [Modest]InvalidOperationException error, string message )
		{
			messageLogger.Exception( message, error );

			// var message = error.ToString();

			Mock.Get( messageLogger ).Verify( x => x.Exception( message, error ) );
		}

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		[DragonSpark.Testing.Framework.Register( typeof( IExceptionFormatter ), typeof( ExceptionFormatter ) )]
		public void Fatal( [Located( false ), Frozen]IMessageLogger messageLogger, IExceptionFormatter formatter, [Modest]InvalidOperationException error, string message )
		{
			// Assert.Same( logger, Log.Current );

			messageLogger.Fatal( message, error );

			// var message = formatter.FormatMessage( error, id );

			Mock.Get( messageLogger ).Verify( x => x.Fatal( message, error ) );
		}

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		[DragonSpark.Testing.Framework.Register( typeof( IExceptionFormatter ), typeof( ExceptionFormatter ) )]
		public void Try()
		{
			var exception = DiagnosticExtensions.Try( () => {} );
			Assert.Null( exception );
		}

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		[DragonSpark.Testing.Framework.Register( typeof( IExceptionFormatter ), typeof( ExceptionFormatter ) )]
		public void TryException( [Located( false ), Frozen, Registered]IMessageLogger messageLogger, [Modest]InvalidOperationException error )
		{
			var exception = DiagnosticExtensions.Try( () => { throw error; } );
			Assert.NotNull( exception );
			Assert.Equal( error, exception );

			// var message = formatter.FormatMessage( error );

			Mock.Get( messageLogger ).Verify( x => x.Exception( "An exception has occurred while executing an application delegate.", exception ) );
		}

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		public void GetAllTypesWith( [Located]AssembliesFactory sut )
		{
			var items = sut.Create().GetAllTypesWith<PriorityAttribute>();
			Assert.True( items.Select( tuple => tuple.Item2 ).AsTypes().Contains( typeof(NormalPriority) ) );
		}

		[Theory, SetupAutoData( typeof(DefaultSetup) ), Test]
		public void Evaluate( ClassWithParameter sut )
		{
			Assert.Equal( sut.Parameter, sut.Evaluate<object>( nameof( sut.Parameter ) ) );
		}

		[Theory, SetupAutoData( typeof(DefaultSetup) ), Test]
		public void Mocked( [Frozen]Mock<IInterface> sut, IInterface item )
		{
			Assert.Equal( sut.Object, item );
		}

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		public void GetAllInstances( IServiceLocator sut )
		{
			Assert.False( sut.GetAllInstances<Class>().Any() );
		}

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		public void Singleton( [Located] IUnityContainer sut )
		{
			var once = sut.Resolve<RegisterAsSingleton>();
			var twice = sut.Resolve<RegisterAsSingleton>();
			Assert.Same( once, twice );
		}

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		public void Many( [Located] IUnityContainer sut )
		{
			var once = sut.Resolve<RegisterAsMany>();
			var twice = sut.Resolve<RegisterAsMany>();
			Assert.NotSame( once, twice );
		}

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		public void Create( [Located]ApplicationInformation sut )
		{
			Assert.NotNull( sut.AssemblyInformation );
			Assert.Equal( DateTimeOffset.Parse( "2/1/2016" ), sut.DeploymentDate.GetValueOrDefault() );
			Assert.Equal( "http://framework.dragonspark.us/testing", sut.CompanyUri.ToString() );
			var assembly = GetType().Assembly;
			Assert.Equal( assembly.FromMetadata<AssemblyTitleAttribute, string>( attribute => attribute.Title ), sut.AssemblyInformation.Title );
			Assert.Equal( assembly.FromMetadata<AssemblyCompanyAttribute, string>( attribute => attribute.Company ), sut.AssemblyInformation.Company );
			Assert.Equal( assembly.FromMetadata<AssemblyCopyrightAttribute, string>( attribute => attribute.Copyright ), sut.AssemblyInformation.Copyright );
			Assert.Equal( assembly.FromMetadata<DebuggableAttribute, string>( attribute => "DEBUG" ), sut.AssemblyInformation.Configuration );
			Assert.Equal( assembly.FromMetadata<AssemblyDescriptionAttribute, string>( attribute => attribute.Description ), sut.AssemblyInformation.Description );
			Assert.Equal( assembly.FromMetadata<AssemblyProductAttribute, string>( attribute => attribute.Product ), sut.AssemblyInformation.Product );
			Assert.Equal( assembly.GetName().Version, sut.AssemblyInformation.Version );
		}

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		public void Factory( AllTypesOfFactory sut )
		{
			var items = sut.Create<IInterface>();
			Assert.True( items.Any() );
			Assert.NotNull( items.FirstOrDefaultOfType<YetAnotherClass>() );
		}

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		public void Locate( [Located]ApplicationAssemblyLocator sut )
		{
			Assert.Same( GetType().Assembly, sut.Create() );
		}

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		public void RegisterWithName( IServiceLocator locator )
		{
			Assert.Null( locator.GetInstance<IRegisteredWithName>() );

			var located = locator.GetInstance<IRegisteredWithName>( "Registered" );
			Assert.IsType<RegisteredWithNameClass>( located );
		}

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		public void CreateAssemblies( AssembliesFactory factory, IUnityContainer container, IAssemblyProvider provider, [Located]Assembly[] sut )
		{
			var registered = container.IsRegistered<Assembly[]>();
			Assert.True( registered );

			var fromFactory = factory.Create();
			var fromContainer = container.Resolve<Assembly[]>();
			Assert.Equal( fromFactory, fromContainer );

			var fromProvider = provider.GetAssemblies();
			Assert.Equal( fromContainer, fromProvider );

			Assert.Equal( fromContainer, sut );
		}

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		public void CreateAssembly( AssemblyInformationFactory factory, IUnityContainer container, IApplicationAssemblyLocator locator, [Located]Assembly sut )
		{
			var registered = container.IsRegistered<Assembly>();
			Assert.True( registered );

			var fromFactory = locator.Create();
			var fromContainer = container.Resolve<Assembly>();
			Assert.Same( fromFactory, fromContainer );

			Assert.Same( fromContainer, sut );

			var fromProvider = factory.Create( fromFactory );
			Assert.NotNull( fromProvider );

			var assembly = GetType().Assembly;
			Assert.Equal( assembly.FromMetadata<AssemblyTitleAttribute, string>( attribute => attribute.Title ), fromProvider.Title );
			Assert.Equal( assembly.FromMetadata<AssemblyCompanyAttribute, string>( attribute => attribute.Company ), fromProvider.Company );
			Assert.Equal( assembly.FromMetadata<AssemblyCopyrightAttribute, string>( attribute => attribute.Copyright ), fromProvider.Copyright );
			Assert.Equal( assembly.FromMetadata<DebuggableAttribute, string>( attribute => "DEBUG" ), fromProvider.Configuration );
			Assert.Equal( assembly.FromMetadata<AssemblyDescriptionAttribute, string>( attribute => attribute.Description ), fromProvider.Description );
			Assert.Equal( assembly.FromMetadata<AssemblyProductAttribute, string>( attribute => attribute.Product ), fromProvider.Product );
			Assert.Equal( assembly.GetName().Version, fromProvider.Version );
		}

		public interface IRegisteredWithName
		{ }

		[Theory, Test, SetupAutoData( typeof(DefaultSetup) )]
		void Register( IAnotherInterface sut )
		{
			Assert.IsType<MultipleInterfaces>( sut );
		}

		[DragonSpark.Setup.Registration.Register( typeof(IAnotherInterface) )]
		public class MultipleInterfaces : IInterface, IAnotherInterface, IItem
		{
			
		}

		interface IAnotherInterface
		{ }

		[DragonSpark.Setup.Registration.Register( "Registered" )]
		public class RegisteredWithNameClass : IRegisteredWithName
		{ }

		class Interfaces
		{
			public Interfaces( IEnumerable<IItem> items )
			{
				Items = items;
			}

			public IEnumerable<IItem> Items { get; }
		}

		public interface IItem
		{ }

		public class Item : IItem
		{ }

		[DragonSpark.Setup.Registration.Register( "AnotherItem" )]
		public class AnotherItem : IItem
		{ }

		[DragonSpark.Setup.Registration.Register( "YetAnotherItem" )]
		public class YetAnotherItem : IItem
		{ }
	}
}
