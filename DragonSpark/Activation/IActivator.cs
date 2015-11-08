using System;

namespace DragonSpark.Activation
{
	public interface IActivator
	{
		bool CanActivate( Type type, string name );

		TResult CreateInstance<TResult>( Type type, string name );

		TResult Create<TResult>( params object[] parameters );
	}
}