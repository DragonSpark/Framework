using System;

namespace DragonSpark.Activation
{
	public interface IActivator
	{
		TResult CreateInstance<TResult>( Type type, string name );

		TResult Create<TResult>( params object[] parameters );
	}
}