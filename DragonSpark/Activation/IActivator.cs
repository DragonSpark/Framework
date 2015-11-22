using DragonSpark.Extensions;
using System;
using System.Linq;

namespace DragonSpark.Activation
{
	public interface IActivator
	{
		bool CanActivate( Type type, string name = null );

		object Activate( Type type, string name = null );

		bool CanConstruct( Type type, params object[] parameters );
		
		object Construct( Type type, params object[] parameters );
	}

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
			var result = activators.Where( activator => activator.CanActivate( type, name ) ).Select( activator => activator.Activate( type, name ) ).NotNull().FirstOrDefault();
			return result;
		}

		public bool CanConstruct( Type type, params object[] parameters )
		{
			var result = activators.Any( activator => activator.CanConstruct( type, parameters ) );
			return result;
		}

		public object Construct( Type type, params object[] parameters )
		{
			var result = activators.Where( activator => activator.CanConstruct( type, parameters ) ).Select( activator => activator.Construct( type, parameters ) ).NotNull().FirstOrDefault();
			return result;
		}
	}

	public static class ActivatorExtensions
	{
		public static bool CanActivate<T>( this IActivator @this, string name = null )
		{
			var result = @this.CanActivate( typeof(T), name );
			return result;
		}
	}
}