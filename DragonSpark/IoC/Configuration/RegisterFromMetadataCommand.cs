using System.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Logging;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.IoC.Configuration
{
	[AttributeUsage( AttributeTargets.Assembly )]
	public class RegistrationAttribute : Attribute
	{
		public RegistrationAttribute( Priority priority )
		{
			Priority = priority;
		}

		public Priority Priority { get; set; }
	}

	public class RegisterFromMetadataCommand : IContainerConfigurationCommand
	{
		readonly Dictionary<Type, Delegate> handlers = new Dictionary<Type, Delegate>();

		IUnityContainer Container { get; set; }

		public string ModuleCatalogBuildName { get; set; }
		public string ModuleManagerBuildName { get; set; }

		public void Configure( IUnityContainer container )
		{
			Container = container;

		    AddRegistrationHandler<SingletonAttribute>( HandleSingleton );
		    AddRegistrationHandler<PerRequestAttribute>( HandlePerRequest );
		    AddRegistrationHandler<InstanceAttribute>( HandleInstance );

            AssemblySupport.Instance.Register( x => x.OrderBy( y => y.FromMetadata<RegistrationAttribute, Priority>( z => z.Priority, () => Priority.Normal ) ).SelectMany( ResolveRegistrations ).Apply( Register ) );
		}

		static IEnumerable<IComponentRegistration> ResolveRegistrations( Assembly item )
		{
			var result = item.GetValidTypes().SelectMany( a => a.GetAttributes<IComponentMetadata>( true ).OrderBy( x => x.Priority ).Select( b => b.GetComponentInfo( a ) ) );
			return result;
		}

		void Register( IComponentRegistration registration )
		{
			var key = registration.GetType();
			Delegate handler;

			if ( handlers.TryGetValue( key, out handler ) )
			{
				handler.DynamicInvoke( registration );
			}
			else
			{
				var exception = new InvalidOperationException( string.Format( "{0} cannot handle registrations of type {1}. Please add an appropriate registration handler.", GetType().FullName, key.FullName ) );
				Log.Error( exception );
				throw exception;
			}
		}

		void AddRegistrationHandler<T>( Action<T> handler ) where T : IComponentRegistration
		{
			handlers[ typeof(T) ] = handler;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Disposed when container is disposed." )]
		void HandleSingleton( SingletonAttribute attribute )
		{
			Trace.WriteLine( string.Format( "'{0}' is performing a singleton registration for type '{1}', with the implementation of '{2}' and name '{3}'", GetType().Name, attribute.Service.FullName, attribute.Implementation.FullName, attribute.Name ) );
			if ( !attribute.HasName() )
			{
				Container.RegisterType( attribute.Service, attribute.Implementation, new Microsoft.Practices.Unity.ContainerControlledLifetimeManager() );
			}
			else
			{
				Container.RegisterType( attribute.HasService() ? attribute.Service : typeof(object), attribute.Implementation, attribute.Name, new Microsoft.Practices.Unity.ContainerControlledLifetimeManager() );
			}
		}

		void HandlePerRequest(PerRequestAttribute attribute)
		{
			Trace.WriteLine( string.Format( "'{0}' is performing a per-request registration for type '{1}', with the implementation of '{2}' and name '{3}'", GetType().Name, attribute.Service.FullName, attribute.Implementation.FullName, attribute.Name ) );
			if ( !attribute.HasName() )
			{
				Container.RegisterType( attribute.Service, attribute.Implementation );
			}
			else if ( !attribute.HasService() )
			{
				Container.RegisterType( typeof(object), attribute.Implementation, attribute.Name );
			}
			else
			{
				Container.RegisterType( attribute.Service, attribute.Implementation, attribute.Name );
			}
		}

		void HandleInstance(InstanceAttribute attribute)
		{
			Trace.WriteLine( string.Format( "'{0}' is registering an instance for type '{1}', with the implementation of '{2}' and name '{3}'", GetType().Name, attribute.Service.FullName, attribute.Implementation, attribute.Name ) );
			if ( !attribute.HasName() )
			{
				Container.RegisterInstance( attribute.Service, attribute.Implementation );
			}
			else if ( !attribute.HasService() )
			{
				Container.RegisterInstance( typeof(object), attribute.Name, attribute.Implementation );
			}
			else
			{
				Container.RegisterInstance( attribute.Service, attribute.Name, attribute.Implementation );
			}
		}
	}
}