using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Properties;
using Microsoft.Practices.Unity;
using System;
using System.Linq;

namespace DragonSpark.Activation.IoC
{
	public class CompositeActivator : IActivator
	{
		readonly ILogger logger;
		readonly IActivator[] activators;

		public CompositeActivator( ILogger logger, params IActivator[] activators )
		{
			this.logger = logger;
			this.activators = activators;
		}

		public bool CanActivate( Type type, string name = null )
		{
			var result = activators.Any( activator => activator.CanActivate( type, name ) );
			return result;
		}

		public object Activate( Type type, string name = null )
		{
			var result = activators.Select( activator => Determine( () => activator.Activate( type, name ), type, name ) ).NotNull().FirstOrDefault();
			return result;
		}

		public object Construct( Type type, params object[] parameters )
		{
			var result = activators.Select( activator => Determine( () => activator.Construct( type, parameters ), type ) ).NotNull().FirstOrDefault();
			return result;
		}

		object Determine<TResult>( Func<TResult> method, Type type, string name = null )
		{
			try
			{
				var result = method();
				return result;
			}
			catch ( Exception e )
			{
				if ( ExceptionExtensions.IsFrameworkExceptionRegistered( e.GetType() ) )
				{
					logger.Warning( string.Format( Resources.Activator_CouldNotActivate, type, name ?? Resources.Activator_None, e.GetMessage() ) );
				}
				else
				{
					throw;
				}
			}
			return null;
		}
	}

	public class Activator : IActivator
	{
		readonly IUnityContainer container;

		public Activator( IUnityContainer container )
		{
			this.container = container;
		}

		public bool CanActivate( Type type, string name )
		{
			var result = container.IsRegistered( type, name );
			return result;
		}

		public object Activate( Type type, string name = null )
		{
			var result = container.Resolve( type, name );
			return result;
		}

		public object Construct( Type type, params object[] parameters )
		{
			var result = container.Create( type, parameters );
			return result;
		}
	}
}