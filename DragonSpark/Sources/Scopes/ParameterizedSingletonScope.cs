using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using System;

namespace DragonSpark.Sources.Scopes
{
	public class ParameterizedSingletonScope<TParameter, TResult> : ParameterizedScope<TParameter, TResult>
	{
		public ParameterizedSingletonScope() : this( parameter => default(TResult) ) {}
		public ParameterizedSingletonScope( TResult instance ) : this( instance.Wrap<TParameter, TResult>() ) {}
		public ParameterizedSingletonScope( Func<TParameter, TResult> factory ) : base( new Func<object, Func<TParameter, TResult>>( Caches.Create( factory.ToSingleton ).Get ) ) {}
		public ParameterizedSingletonScope( Func<object, Func<TParameter, TResult>> global ) : base( global.ToSingleton() ) {}

		public override void Assign( Func<Func<TParameter, TResult>> item ) => base.Assign( item.ToSingleton() );
		public override void Assign( Func<object, Func<TParameter, TResult>> item ) => base.Assign( item.ToSingleton() );
	}
}