using System;

namespace DragonSpark.Sources.Parameterized
{
	public class ParameterizedScope<T> : ParameterizedScope<object, T>, IParameterizedScope<T>
	{
		public ParameterizedScope( Func<object, T> source ) : base( source ) {}
		public ParameterizedScope( Func<object, Func<object, T>> source ) : base( source ) {}
		protected ParameterizedScope( IScope<Func<object, T>> scope ) : base( scope ) {}
	}

	public class ParameterizedScope<TParameter, TResult> : ParameterizedSourceBase<TParameter, TResult>, IParameterizedScope<TParameter, TResult>
	{
		readonly IScope<Func<TParameter, TResult>> scope;

		public ParameterizedScope( Func<TParameter, TResult> source ) : this( new Func<object, Func<TParameter, TResult>>( new Wrapper<object, Func<TParameter, TResult>>( source.Self ).Get ) ) {}

		public ParameterizedScope( Func<object, Func<TParameter, TResult>> source ) : this( new Scope<Func<TParameter, TResult>>( source ) ) {}

		protected ParameterizedScope( IScope<Func<TParameter, TResult>> scope )
		{
			this.scope = scope;
		}

		public override TResult Get( TParameter parameter ) => scope.Get().Invoke( parameter );

		public void Assign( ISource item ) => scope.Assign( item );

		public virtual void Assign( Func<object, Func<TParameter, TResult>> item ) => scope.Assign( item );
		public virtual void Assign( Func<Func<TParameter, TResult>> item ) => scope.Assign( item );
	}
}