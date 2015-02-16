using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Markup;
using AutoMapper;
using DragonSpark.Activation.IoC;
using DragonSpark.Activation.IoC.Commands;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using Dynamitey;
using EventSourceProxy;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Application.IoC.Commands
{
	[AttributeUsage( AttributeTargets.Assembly, AllowMultiple = true )]
	public class IgnoreNamespaceDuringRegistrationAttribute : Attribute
	{
		readonly string ns;

		public IgnoreNamespaceDuringRegistrationAttribute( Type type ) : this( type.Namespace )
		{}

		public IgnoreNamespaceDuringRegistrationAttribute( string @namespace )
		{
			ns = @namespace;
		}

		public string Namespace
		{
			get { return ns; }
		}
	}

	public class AssemblyLocator : IAssemblyLocator
	{
		public IEnumerable<Assembly> GetAllAssemblies()
		{
			return AppDomain.CurrentDomain.GetAssemblies();
		}
	}

	[ContentProperty( "AssemblyNames" )]
	public class ApplyRegistrationsFromConventionsCommand : IContainerConfigurationCommand
	{
		public ApplyRegistrationsFromConventionsCommand()
		{
			IgnoreNamespaces = string.Join( ";", new[] { typeof(PartialApply), typeof(Notification), typeof(EventAggregator), typeof(Region), typeof(Mapper), typeof(ModuleCatalog), typeof(IUnityContainerTypeConfiguration) }.Select( x => x.Namespace ).ToArray() );
		}

		public string AssemblyNamesStartsWith { get; set; }

		public string IgnoreNamespaces { get; set; }

		public void Configure( IUnityContainer container )
		{
			var locator = new AssemblyLocator();

			var names = AssemblyNamesStartsWith.ToStringArray();
			var namespaces = IgnoreNamespaces.ToStringArray().Concat( locator.GetAllAssemblies().SelectMany( x => x.GetCustomAttributes<IgnoreNamespaceDuringRegistrationAttribute>().Select( y => y.Namespace ) ) ).ToArray();
			var types = AllClasses.FromAssembliesInBasePath().Where( x => x.Namespace != null )
				.Where( x => !names.Any() || names.Any( y => x.Assembly.GetName().Name.StartsWith( y, StringComparison.InvariantCultureIgnoreCase ) ) )
				.Where( x => namespaces.All( y => !x.Namespace.StartsWith( y, StringComparison.InvariantCultureIgnoreCase ) ) )
				.Where( x => WithMappings.FromMatchingInterface( x ).Any( y => y.IsPublic ) )
				.OrderBy( x => x.Assembly.FromMetadata<RegistrationAttribute, Priority>( z => z.Priority, () => Priority.Normal ) ).ToArray();

			container.RegisterTypes( types, WithMappings.FromMatchingInterface, WithName.Default, DetermineLifetimeContainer, overwriteExistingMappings: true );
		}

		static LifetimeManager DetermineLifetimeContainer( Type type )
		{
			var result = type.FromMetadata<LifetimeManagerAttribute, LifetimeManager>( x => Activator.CreateInstance<LifetimeManager>( x.LifetimeManagerType ) ) ?? new ContainerControlledLifetimeManager();
			return result;
		}
	}

	[AttributeUsage( AttributeTargets.Class )]
	public class LifetimeManagerAttribute : Attribute
	{
		public Type LifetimeManagerType { get; private set; }

		public LifetimeManagerAttribute( Type lifetimeManagerType )
		{
			LifetimeManagerType = lifetimeManagerType;
		}
	}

	[ContentProperty( "Definitions" )]
	public class LoggingConfiguration : IContainerConfigurationCommand
	{
		[Default( true )]
		public bool EnableAll { get; set; }

		public void Configure( IUnityContainer container )
		{
			container.TryResolve<IEventListenerRegistry>().With( x =>
			{
				Definitions.Where( y => y.Listener != null ).Apply( y =>
				{
					y.Registrations.Apply( z => x.Register( y.Listener, z.Key, z.Value ?? new EventSourceRegistration() ) );
				} );

				var types = Definitions.SelectMany( y => y.Registrations.Keys ).Distinct().ToArray();
				types.Apply( z =>
				{
					var source = EventSourceImplementer.GetEventSource( z );

					// TracingProxy.CreateWithActivityScope<>()
					z.GetTypeInfo().GetAllInterfaces().Apply( a => container.RegisterInstance( a, source ) );
				} );
				EnableAll.IsTrue( x.EnableAll );
			} );
		}

		public Collection<EventListenerDefinition> Definitions
		{
			get { return definitions; }
		}	readonly Collection<EventListenerDefinition> definitions = new Collection<EventListenerDefinition>();
	}

	[ContentProperty( "Listener" )]
	public class EventListenerDefinition
	{
		public EventListener Listener { get; set; }

		public IDictionary<Type, EventSourceRegistration> Registrations { get; set; }
	}

	public interface IEventListenerRegistry
	{
		void Register( EventListener listener, Type eventSourceType, EventSourceRegistration registration );

		IEnumerable<EventListener> GetAll();

		IEnumerable<KeyValuePair<Type, EventSourceRegistration>> Retrieve( EventListener listener );
	}

	public static class EventListenerRegistryExtensions
	{
		public static void EnableAll( this IEventListenerRegistry target )
		{
			target.GetAll().Apply( x => target.Retrieve( x ).Apply( y => x.ListenTo( y.Key, y.Value.Level, y.Value.Keywords ) ) );
		}

		public static void DisableAll( this IEventListenerRegistry target )
		{
			target.GetAll().Apply( x => target.Retrieve( x ).Apply( y => x.Ignore( y.Key ) ) );
		}
	}

	public static class EventListenerExtensions
	{
		public static T ListenTo<T>( this EventListener target, EventLevel level = EventLevel.LogAlways, EventKeywords keywords = EventKeywords.None ) where T : class
		{
			var result = ListenTo( target, typeof(T), level, keywords );
			return (T)result;
		}

		public static object ListenTo( this EventListener target, Type eventSourceType, EventLevel level, EventKeywords keywords = EventKeywords.None )
		{
			var result = EventSourceImplementer.GetEventSource( eventSourceType );
			result.As<EventSource>( x => target.EnableEvents( x, level, keywords ) );
			return result;
		}

		public static T Ignore<T>( this EventListener target ) where T : class
		{
			var result = Ignore( target, typeof(T) );
			return (T)result;
		}

		public static object Ignore( this EventListener target, Type eventSourceType )
		{
			var result = EventSourceImplementer.GetEventSource( eventSourceType );
			result.As<EventSource>( target.DisableEvents );
			return result;
		}
	}

	public class EventListenerRegistry : IEventListenerRegistry
	{
		readonly IDictionary<EventListener, IDictionary<Type, EventSourceRegistration>> cache = new Dictionary<EventListener, IDictionary<Type, EventSourceRegistration>>();

		public void Register( EventListener listener, Type eventSourceType, EventSourceRegistration registration )
		{
			Ensure( listener )[ eventSourceType ] = registration;
		}

		public IEnumerable<EventListener> GetAll()
		{
			var result = cache.Keys.ToArray();
			return result;
		}

		public IEnumerable<KeyValuePair<Type,EventSourceRegistration>> Retrieve( EventListener listener )
		{
			var result = Ensure( listener ).ToArray();
			return result;
		}

		IDictionary<Type, EventSourceRegistration> Ensure( EventListener listener )
		{
			var result = cache.Ensure( listener, x => new Dictionary<Type, EventSourceRegistration>() );
			return result;
		}
	}

	public class EventSourceRegistration
	{
		public EventSourceRegistration()
		{
			Level = EventLevel.LogAlways;
			Keywords = (EventKeywords)(-1);
		}

		public EventLevel Level { get; set; }

		public EventKeywords Keywords { get; set; }
	}

	[ContentProperty( "Policies" )]
	public class ExceptionHandlingConfiguration : IContainerConfigurationCommand
	{
		public void Configure( IUnityContainer container )
		{
			ExceptionPolicy.Reset();

			var manager = new ExceptionManager( Policies );
			container.RegisterInstance( manager );

			ExceptionPolicy.SetExceptionManager( manager );

			var exceptionHandler = container.TryResolve<Diagnostics.IExceptionHandler>();
			exceptionHandler.NotNull( ConfigureExceptionHandling );
		}

		protected virtual void ConfigureExceptionHandling( Diagnostics.IExceptionHandler handler )
		{
			TaskScheduler.UnobservedTaskException += ( sender, args ) => handler.Process( args.Exception );
			AppDomain.CurrentDomain.With( x => x.UnhandledException += ( s, args ) =>
			{
				args.ExceptionObject.As<Exception>( handler.Process );
			} );
		}

		public Collection<Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionPolicyDefinition> Policies
		{
			get { return policies; }
		}	readonly Collection<Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionPolicyDefinition> policies = new Collection<Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionPolicyDefinition>();
	}

	[ContentProperty( "Entries" )]
	public class ExceptionPolicyDefinition : MarkupExtension
	{
		public string PolicyName { get; set; }

		public Collection<Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionPolicyEntry> Entries
		{
			get { return entries; }
		}	readonly Collection<Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionPolicyEntry> entries = new Collection<Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionPolicyEntry>();

		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var result = new Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionPolicyDefinition( PolicyName, Entries );
			return result;
		}
	}

	[ContentProperty( "Handlers" )]
	public class ExceptionPolicyEntry : MarkupExtension
	{
		public Type ExceptionType { get; set; }

		public PostHandlingAction Action { get; set; }

		public Collection<IExceptionHandler> Handlers
		{
			get { return handlers; }
		}	readonly Collection<IExceptionHandler> handlers = new Collection<IExceptionHandler>();

		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var result = new Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionPolicyEntry( ExceptionType, Action, Handlers );
			return result;
		}
	}

	public static class ExceptionHandlerExtensions
	{
		public static void Process( this Diagnostics.IExceptionHandler target, Exception exception )
		{
			target.Handle( exception ).With( a => a.RethrowRecommended.IsTrue( () => { throw a.Exception; } ) );
		}
	}
}