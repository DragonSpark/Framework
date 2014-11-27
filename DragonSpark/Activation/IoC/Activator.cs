using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using System;

namespace DragonSpark.Activation.IoC
{
	class Activator : IActivator
	{
		readonly IUnityContainer container;

		public Activator( IUnityContainer container )
		{
			this.container = container;
		}

		public TResult CreateInstance<TResult>( Type type, string name = null )
		{
			var result = Determine( 
				() => container.Resolve( type, name ).To<TResult>(), 
				() => SystemActivator.Instance.CreateInstance<TResult>( type, name )
			);
			return result;
		}

		static TResult Determine<TResult>( Func<TResult> method, Func<TResult> backup )
		{
			try
			{
				var result = method();
				return result;
			}
			catch ( ResolutionFailedException e )
			{
				// Log.Warning( string.Format( Resources.Activator_CouldNotActivate, e.TypeRequested, e.NameRequested ?? Resources.Activator_None, e.GetMessage() ) );
				return backup();
			}
		}

		public TResult Create<TResult>( params object[] parameters )
		{
			var result = Determine( 
				() => container.Create<TResult>( parameters ), 
				() => SystemActivator.Instance.Create<TResult>( parameters ) 
				);
			return result;
		}
	}
}