using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Properties;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Activation.IoC
{
	public class ServiceLocator : ServiceLocatorImplBase, IServiceRegistry, IObjectBuilder, IDisposable
	{
		readonly IUnityContainer container;
		readonly ConditionMonitor disposed = new ConditionMonitor();

		public ServiceLocator() : this( new UnityContainer() )
		{}

		public ServiceLocator( IUnityContainer container )
		{
			this.container = container;
			this.container.RegisterInstance<IServiceLocator>( this );
			this.container.RegisterInstance<IServiceRegistry>( this );
			this.container.RegisterInstance<IObjectBuilder>( this );
		}

		public override IEnumerable<TService> GetAllInstances<TService>()
		{
			var enumerable = Container.IsRegistered<IEnumerable<TService>>() ? Container.Resolve<IEnumerable<TService>>() : Enumerable.Empty<TService>();
			var result = base.GetAllInstances<TService>().Union( enumerable ).ToArray();
			return result;
		}

		protected override IEnumerable<object> DoGetAllInstances( Type serviceType )
		{
			var result = Container.ResolveAll( serviceType ).ToArray();
			return result;
		}

		protected override object DoGetInstance( Type serviceType, string key )
		{
			if ( Container.IsResolvable( serviceType, key ) )
			{
				try
				{
					var result = Container.Resolve( serviceType, key );
					return result;
				}
				catch ( ResolutionFailedException e )
				{
					Warn( serviceType, string.Format( Resources.Activator_CouldNotActivate, e.TypeRequested, e.NameRequested ?? Resources.Activator_None, e.GetMessage() ) );
					return null;
				}
			}

			Warn( serviceType, string.Format( Resources.ServiceLocator_NotRegistered, serviceType, key ?? Resources.Activator_None ) );
			return null;
		}

		static void Warn( Type type, string message )
		{
			typeof(ILogger).IsAssignableFrom( type ).IsFalse( () => Log.Current.Warning( message ) );
		}

		public IUnityContainer Container
		{
			get
			{
				switch ( disposed.State )
				{
					case ConditionMonitorState.Applied:
						throw new ObjectDisposedException( Resources.ServiceLocator_Container );
				}
				return container;
			}
		}

		public void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		protected virtual void Dispose( bool disposing )
		{
			disposed.Apply( () =>
			{
				Services.Location.With( item =>
				{
					if ( item.IsAvailable && item.Locator == this )
					{
						item.Assign( null );
					}
				} );

				Container.DisposeAll();
			} );
		}

		public void Register( Type @from, Type mappedTo, string name = null )
		{
			container.RegisterType( from, mappedTo, name );
		}

		public void Register( Type type, object instance )
		{
			container.RegisterInstance( type, instance );
		}

		public void RegisterFactory( Type type, Func<object> factory )
		{
			container.RegisterType( type, new InjectionFactory( x =>
			{
				var item = factory();
				return item;
			} ) );
		}

		public object BuildUp( object item )
		{
			var result = Container.BuildUp( item.GetType(), item );
			return result;
		}
	}
}