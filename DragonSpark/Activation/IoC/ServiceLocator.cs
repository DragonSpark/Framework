using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Properties;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Activation.IoC
{
	public class ServiceLocator : ServiceLocatorImplBase, IDisposable
	{
		readonly IUnityContainer container;
		readonly ConditionMonitor disposed = new ConditionMonitor();

		public ServiceLocator() : this( new UnityContainer() )
		{}

		public ServiceLocator( IUnityContainer container )
		{
			this.container = container;
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
			var basic = key == null && !serviceType.GetTypeInfo().IsInterface;
			if ( basic || Container.IsRegistered( serviceType, key ) )
			{
				return new ResolutionContext( MessageLogger ).Execute( () => Container.Resolve( serviceType, key ) );
			}

			MessageLogger.Warning( string.Format( Resources.ServiceLocator_NotRegistered, serviceType, key ?? Resources.Activator_None ) );
			return null;
		}

		IMessageLogger MessageLogger => Container.DetermineLogger();

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
			disposed.Apply( container.Dispose );
		}
	}
}