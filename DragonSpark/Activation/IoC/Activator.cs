using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Properties;
using Microsoft.Practices.Unity;
using System;

namespace DragonSpark.Activation.IoC
{
	public class Activator : IActivator
	{
		readonly IUnityContainer container;
		readonly ILogger logger;

		public Activator( IUnityContainer container ) : this( container, container.Resolve<ILogger>() )
		{}

		public Activator( IUnityContainer container, ILogger logger )
		{
			this.container = container;
			this.logger = logger;
		}

		public bool CanActivate( Type type, string name )
		{
			var result = container.IsRegistered( type, name );
			return result;
		}

		public object Activate( Type type, string name = null )
		{
			var result = Determine( 
				() => container.Resolve( type, name ),
				() => SystemActivator.Instance.Activate( type, name )
			);
			return result;
		}

		public object Construct( Type type, params object[] parameters )
		{
			var result = Determine( 
				() => container.Create( type, parameters ),
				() => SystemActivator.Instance.Construct( type, parameters )
			);
			return result;
		}

		TResult Determine<TResult>( Func<TResult> method, Func<TResult> backup )
		{
			try
			{
				var result = method();
				return result;
			}
			catch ( ResolutionFailedException e )
			{
				logger.Warning( string.Format( Resources.Activator_CouldNotActivate, e.TypeRequested, e.NameRequested ?? Resources.Activator_None, e.GetMessage() ) );
				return backup();
			}
		}
	}
}