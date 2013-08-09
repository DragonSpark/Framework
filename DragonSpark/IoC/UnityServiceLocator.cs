using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.IoC
{
	public class UnityServiceLocator : ServiceLocatorImplBase
	{
		readonly IUnityContainer container;
		readonly BitFlipper disposed = new BitFlipper();

		public UnityServiceLocator( IUnityContainer container )
		{
			this.container = container;
			this.container.RegisterInstance<IServiceLocator>( this );
		}

		protected override IEnumerable<object> DoGetAllInstances( Type serviceType )
		{
			if ( container == null )
			{
				throw new ObjectDisposedException( "container" );
			}
			return container.ResolveAll( serviceType, new ResolverOverride[0] );
		}

		protected override object DoGetInstance( Type serviceType, string key )
		{
			if ( container == null )
			{
				throw new ObjectDisposedException( "container" );
			}
			var result = container.IsResolvable( serviceType, key ) ? container.Resolve( serviceType, key, new ResolverOverride[0] ) : null;
			return result;
		}

		public IUnityContainer Container
		{
			get { return container; }
		}

		public void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		protected virtual void Dispose( bool disposing )
		{
			disposed.Check( () =>
			{
				var extension = Container.Configure<DragonSparkExtension>();
				var lifetimeContainer = extension.LifetimeContainer;
				var entries = Container.GetLifetimeEntries().Where( x => x.Value == this || x.Value == Container ).Select( x => x.Key ).ToArray();
				entries.Apply( lifetimeContainer.Remove );
				extension.Children.ToArray().Apply( x => x.DisposeAll() );
				Container.DisposeAll();

				ServiceLocator.SetLocatorProvider( () => null );
			} );
		}
	}
}