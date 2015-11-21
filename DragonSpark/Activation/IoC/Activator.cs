using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using System;
using System.Linq;

namespace DragonSpark.Activation.IoC
{
	public class CompositeActivator : IActivator
	{
		readonly IActivator[] activators;

		public CompositeActivator( params IActivator[] activators )
		{
			this.activators = activators;
		}

		public bool CanActivate( Type type, string name = null )
		{
			var result = activators.Any( activator => activator.CanActivate( type, name ) );
			return result;
		}

		public object Activate( Type type, string name = null )
		{
			var result = activators.Select( activator => activator.Activate( type, name ) ).NotNull().FirstOrDefault();
			return result;
		}

		public object Construct( Type type, params object[] parameters )
		{
			var result = activators.Select( activator => activator.Construct( type, parameters ) ).NotNull().FirstOrDefault();
			return result;
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
			var result = container.ResolveWithContext( type, name );
			return result;
		}

		public object Construct( Type type, params object[] parameters )
		{
			var result = container.Create( type, parameters );
			return result;
		}
	}
}