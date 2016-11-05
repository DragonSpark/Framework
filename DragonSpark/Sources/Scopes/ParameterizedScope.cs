using DragonSpark.Sources.Parameterized;
using JetBrains.Annotations;
using System;

namespace DragonSpark.Sources.Scopes
{
	public class ParameterizedScope<TParameter, TResult> : ParameterizedSourceBase<TParameter, TResult>, IParameterizedScope<TParameter, TResult>
	{
		readonly IScope<Func<TParameter, TResult>> scope;

		public ParameterizedScope() : this( parameter => default(TResult) ) {}
		public ParameterizedScope( Func<TParameter, TResult> source ) : this( new Scope<Func<TParameter, TResult>>( source.Invoke ) ) {}

		public ParameterizedScope( Func<object, Func<TParameter, TResult>> source ) : this( new Scope<Func<TParameter, TResult>>( source ) ) {}

		[UsedImplicitly]
		protected ParameterizedScope( IScope<Func<TParameter, TResult>> scope )
		{
			this.scope = scope;
		}

		public override TResult Get( TParameter parameter ) => GetFactory().Invoke( parameter );

		public Func<TParameter, TResult> GetFactory() => scope.Get();

		public void Assign( ISource item ) => scope.Assign( item );

		public virtual void Assign( Func<object, Func<TParameter, TResult>> item ) => scope.Assign( item );
		public virtual void Assign( Func<Func<TParameter, TResult>> item ) => scope.Assign( item );
	}
}