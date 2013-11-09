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

            Container.Resolve<AssemblySupport>().RegisterAndApply( x =>
            {
	            var assemblies = x.OrderBy( y => y.FromMetadata<RegistrationAttribute, Priority>( z => z.Priority, () => Priority.Normal ) ).ToArray();
	            assemblies.SelectMany( ResolveRegistrations ).Apply( Register );
            } );
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
				Logging.Log.Error( exception );
				throw exception;
			}
		}

		void AddRegistrationHandler<T>( Action<T> handler ) where T : IComponentRegistration
		{
			handlers[ typeof(T) ] = handler;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Disposed when container is disposed." )]
		void HandleSingleton( SingletonAttribute singletonAttribute )
		{
			if ( !singletonAttribute.HasName() )
			{
				Container.RegisterType( singletonAttribute.Service, singletonAttribute.Implementation, new Microsoft.Practices.Unity.ContainerControlledLifetimeManager() );
			}
			else
			{
				Container.RegisterType( singletonAttribute.HasService() ? singletonAttribute.Service : typeof(object), singletonAttribute.Implementation, singletonAttribute.Name, new Microsoft.Practices.Unity.ContainerControlledLifetimeManager() );
			}
		}

		void HandlePerRequest(PerRequestAttribute perRequestAttribute)
		{
			if ( !perRequestAttribute.HasName() )
			{
				Container.RegisterType( perRequestAttribute.Service, perRequestAttribute.Implementation );
			}
			else if ( !perRequestAttribute.HasService() )
			{
				Container.RegisterType( typeof(object), perRequestAttribute.Implementation, perRequestAttribute.Name );
			}
			else
			{
				Container.RegisterType( perRequestAttribute.Service, perRequestAttribute.Implementation, perRequestAttribute.Name );
			}
		}

		void HandleInstance(InstanceAttribute instanceAttribute)
		{
			if ( !instanceAttribute.HasName() )
			{
				Container.RegisterInstance( instanceAttribute.Service, instanceAttribute.Implementation );
			}
			else if ( !instanceAttribute.HasService() )
			{
				Container.RegisterInstance( typeof(object), instanceAttribute.Name, instanceAttribute.Implementation );
			}
			else
			{
				Container.RegisterInstance( instanceAttribute.Service, instanceAttribute.Name, instanceAttribute.Implementation );
			}
		}
	}
}