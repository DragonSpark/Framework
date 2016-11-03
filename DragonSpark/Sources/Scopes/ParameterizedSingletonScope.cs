using System;

namespace DragonSpark.Sources.Scopes
{
	public class ParameterizedSingletonScope<TParameter, TResult> : ParameterizedScope<TParameter, TResult>
	{
		// public ParameterizedSingletonScope() : this( parameter => default(TResult) ) {}
		public ParameterizedSingletonScope( Func<TParameter, TResult> factory ) : base( factory.ToGlobalSingleton() ) {}
		public ParameterizedSingletonScope( Func<object, Func<TParameter, TResult>> global ) : base( global.ToSingleton() ) {}
	}
}