using System;

namespace DragonSpark.Sources.Scopes
{
	public class ParameterizedScopedSingleton<TParameter, TResult> : ParameterizedScope<TParameter, TResult>
	{
		public ParameterizedScopedSingleton() : this( parameter => default(TResult) ) {}
		public ParameterizedScopedSingleton( Func<TParameter, TResult> factory ) : this( factory.Singleton() ) {}
		public ParameterizedScopedSingleton( Func<object, Func<TParameter, TResult>> global ) : base( global.Cache() ) {}
	}
}