﻿using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Properties;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DragonSpark.Setup;

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
			this.container
				.RegisterInstance<IServiceLocator>( this )
				.Extension<IoCExtension>();
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
				return new ResolutionContext( Logger ).Execute( () => Container.Resolve( serviceType, key ) );
			}

			Logger.Warning( string.Format( Resources.ServiceLocator_NotRegistered, serviceType, key ?? Resources.Activator_None ) );
			return null;
		}

		ILogger Logger => Container.DetermineLogger();

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

				Container.Dispose();
			} );
		}
	}
}