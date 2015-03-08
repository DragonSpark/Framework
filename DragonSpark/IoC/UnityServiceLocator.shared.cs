using System;
using System.Linq;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace DragonSpark.IoC
{
	public class UnityServiceLocator : UnityServiceLocatorAdapter, IDisposable
	{
		readonly IUnityContainer container;
		readonly BitFlipper disposed = new BitFlipper();
		
		public UnityServiceLocator( IUnityContainer container ) : base( container )
		{
			this.container = container;
			this.container.RegisterInstance<IServiceLocator>( this );
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