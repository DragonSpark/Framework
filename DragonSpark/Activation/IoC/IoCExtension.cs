using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Runtime.Specifications;
using DragonSpark.Setup.Registration;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;
using System;
using LifetimeManagerFactory = DragonSpark.Setup.Registration.LifetimeManagerFactory;

namespace DragonSpark.Activation.IoC
{
	public class IoCExtension : UnityContainerExtension
	{
		public ILogger Logger { get; } = new RecordingLogger();

		protected override void Initialize()
		{
			Context.Policies.SetDefault<IConstructorSelectorPolicy>( DefaultUnityConstructorSelectorPolicy.Instance );

			Context.Strategies.Clear();
			Context.Strategies.AddNew<BuildKeyMappingStrategy>( UnityBuildStage.TypeMapping );
			Context.Strategies.AddNew<HierarchicalLifetimeStrategy>( UnityBuildStage.Lifetime );
			Context.Strategies.AddNew<LifetimeStrategy>( UnityBuildStage.Lifetime );
			Context.Strategies.AddNew<ArrayResolutionStrategy>( UnityBuildStage.Creation );
			Context.Strategies.AddNew<EnumerableResolutionStrategy>( UnityBuildStage.Creation );
			Context.Strategies.AddNew<BuildPlanStrategy>( UnityBuildStage.Creation );
			Context.Strategies.AddNew<ObjectBuilderStrategy>( UnityBuildStage.Initialization );

			Container.RegisterInstance<IResolutionSupport>( new ResolutionSupport( Context ) );

			Container.Registration<EnsuredRegistrationSupport>().With( support =>
			{
				support.Instance( CreateActivator );
				support.Instance( CreateRegistry );
			});
		}

		IServiceRegistry CreateRegistry()
		{
			var factory = new LifetimeManagerFactory( Container.Resolve<IActivator>() );
			var result = new ServiceRegistry( Container, Container.DetermineLogger(), factory );
			return result;
		}

		IActivator CreateActivator()
		{
			var result = new CompositeActivator( Container.Resolve<Activator>(), SystemActivator.Instance );
			return result;
		}

		/*void ContextChildContainerCreated(object sender, ChildContainerCreatedEventArgs e)
		{
			e.ChildContainer.With( child =>
			{
				Children.Add( child );
				child.RegisterInstance( Container, new ContainerLifetimeManager() );
				child.RegisterInstance( new ChildContainerListener( this, child ) );
			} );
		}

		class ContainerLifetimeManager : LifetimeManager
		{
			object value;

			public override object GetValue()
			{
				return value;
			}

			public override void SetValue(object newValue)
			{
				value = newValue;
			}

			public override void RemoveValue()
			{}
		}

		class ChildContainerListener : IDisposable
		{
			readonly IoCExtension extension;
			readonly IUnityContainer child;

			public ChildContainerListener( IoCExtension extension, IUnityContainer child )
			{
				this.extension = extension;
				this.child = child;
			}

			void IDisposable.Dispose()
			{
				extension.Children.Remove( child );
				var container = extension.Context.Container.GetLifetimeContainer();
				var lifetimeEntries = child.GetLifetimeEntries().Where( x => x.Value == child );
				lifetimeEntries.Each( container.Remove );
			}
		}

		public override void Remove()
		{
			Context.ChildContainerCreated -= ContextChildContainerCreated;
		}*/

		/*public IUnityContainer Dispose()
		{
			var entries = GetLifetimeEntries().Where( x => x.Value == Container ).Select( x => x.Key ).ToArray();
			entries.Each( Context.Lifetime.Remove );
			//Children.ToArray().Each( y => y.DisposeAll() );
			Container.Dispose();
			Container.RemoveAllExtensions();
			return Container;
		}*/

		/*public IEnumerable<LifetimeEntry> GetLifetimeEntries()
		{
			var result = Context.Lifetime.Select( ResolveLifetimeEntry ).ToArray();
			return result;
		}

		static LifetimeEntry ResolveLifetimeEntry( object x )
		{
			var result = x.AsTo<ILifetimePolicy, LifetimeEntry>( y => new LifetimeEntry( y, y.GetValue() ), () => new LifetimeEntry( x ) );
			return result;
		}*/

		public IUnityContainer Register( IServiceLocator locator )
		{
			return Container.Registration().Instance( locator );
		}
	}

	class ServiceLocationMonitor : IDisposable
	{
		readonly IServiceLocation location;
		readonly IServiceLocator locator;

		/*public ServiceLocationMonitor( IServiceLocator locator ) : this( Services.Location, locator )
		{}*/

		public ServiceLocationMonitor( IServiceLocation location, IServiceLocator locator )
		{
			this.location = location;
			this.locator = locator;
		}

		public void Dispose()
		{
			if ( location.IsAvailable && location.Locator == locator )
			{
				location.Assign( null );
			}
		}
	}

	public class RegistrationSpecificationParameter
	{
		public RegistrationSpecificationParameter( IUnityContainer container, Type type )
		{
			Container = container;
			Type = type;
		}

		public IUnityContainer Container { get; }
		public Type Type { get; }
	}

	public class NotRegisteredSpecification : SpecificationBase<RegistrationSpecificationParameter>
	{
		public static NotRegisteredSpecification Instance { get; } = new NotRegisteredSpecification();

		protected override bool IsSatisfiedByContext( RegistrationSpecificationParameter context )
		{
			var result = base.IsSatisfiedByContext( context) && !context.Container.IsRegistered( context.Type );
			return result;
		}
	}

	
	public class EnsuredRegistrationSupport : RegistrationSupport
	{
		public EnsuredRegistrationSupport( IUnityContainer container ) : base( container, NotRegisteredSpecification.Instance )
		{}
	}

	[Register]
	public class RegistrationSupport
	{
		readonly IUnityContainer container;
		readonly ISpecification<RegistrationSpecificationParameter> specification;

		public RegistrationSupport( IUnityContainer container ) : this( container, AlwaysSpecification<RegistrationSpecificationParameter>.Instance )
		{}

		protected RegistrationSupport( IUnityContainer container, ISpecification<RegistrationSpecificationParameter> specification )
		{
			this.container = container;
			this.specification = specification;
		}

		public IUnityContainer Convention( object instance )
		{
			return Check( instance.GetType(), () => container.RegisterInstance( instance.Extend().GetConventionCandidate(), instance ) );
		}

		IUnityContainer Check( Type type, Action apply )
		{
			specification.IsSatisfiedBy( new RegistrationSpecificationParameter( container, type ) ).IsTrue( apply );
			return container;
		}

		public IUnityContainer AllInterfaces( object instance )
		{
			return Check( instance.GetType(), () => instance.Extend().GetAllInterfaces().Each( y => container.RegisterInstance( y, instance ) ) );
		}

		public IUnityContainer AllClasses( object instance )
		{
			return Check( instance.GetType(), () => instance.Extend().GetAllHierarchy().Each( y => container.RegisterInstance( y, instance ) ) );
		}

		public IUnityContainer Mapping<TInterface, TImplementation>( LifetimeManager manager = null ) where TImplementation : TInterface
		{
			return Check( typeof(TInterface), () => container.RegisterType<TInterface, TImplementation>( manager ?? new TransientLifetimeManager() ) );
		}

		public IUnityContainer Instance( Type type, object instance, LifetimeManager manager = null )
		{
			return Instance( type, () => instance, manager );
		}

		public IUnityContainer Instance( Type type, Func<object> instance, LifetimeManager manager = null )
		{
			return Check( type, () => container.RegisterInstance( type, instance(), manager ?? new ContainerControlledLifetimeManager() ) );
		}

		public IUnityContainer Instance<TInterface>( TInterface instance, LifetimeManager manager = null )
		{
			return Instance( () => instance, manager );
		}

		public IUnityContainer Instance<TInterface>( Func<TInterface> instance, LifetimeManager manager = null )
		{
			return Check( typeof(TInterface), () => container.RegisterInstance( typeof(TInterface), instance(), manager ?? new ContainerControlledLifetimeManager() ) );
		}
	}
}