using System;

namespace DragonSpark.Runtime
{
	public interface IActivator
	{
		TResult CreateInstance<TResult>( Type type, string name );

		TResult Create<TResult>( params object[] parameters );
	}
}