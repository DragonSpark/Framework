using System;

namespace DragonSpark.Activation
{
	public interface IActivator
	{
		bool CanActivate( Type type, string name = null );

		object Activate( Type type, string name = null );

		object Construct( Type type, params object[] parameters );
	}
}