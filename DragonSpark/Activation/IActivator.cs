using System;

namespace DragonSpark.Activation
{
	public interface IActivator
	{
		bool CanActivate( Type type, string name = null );

		object Activate( Type type, string name = null );

		bool CanConstruct( Type type, params object[] parameters );
		
		object Construct( Type type, params object[] parameters );
	}

	public static class ActivatorExtensions
	{
		public static bool CanActivate( this IActivator @this, Type type )
		{
			var result = @this.CanActivate( type );
			return result;
		}
	}
}