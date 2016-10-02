using DragonSpark.TypeSystem;
using System;

namespace DragonSpark.Sources.Parameterized
{
	public class ConfiguringFactory<T> : DelegatedSource<T>
	{
		readonly Action initialize;
		readonly Action<T> configure;

		public ConfiguringFactory( Func<T> inner, Action<T> configure ) : this( inner, TypeSystem.Delegates.Empty, configure ) {}

		public ConfiguringFactory( Func<T> inner, Action initialize ) : this( inner, initialize, Delegates<T>.Empty ) {}

		public ConfiguringFactory( Func<T> inner, Action initialize, Action<T> configure ) : base( inner )
		{
			this.initialize = initialize;
			this.configure = configure;
		}

		public override T Get()
		{
			initialize();
			var result = base.Get();
			configure( result );
			return result;
		}
	}

	public class ConfiguringFactory<TParameter, TResult> : DelegatedParameterizedSource<TParameter, TResult>
	{
		readonly Action<TParameter> initialize;
		readonly Action<TResult> configure;

		public ConfiguringFactory( Func<TParameter, TResult> factory, Action<TResult> configure ) : this( factory, Delegates<TParameter>.Empty, configure ) {}

		public ConfiguringFactory( Func<TParameter, TResult> factory, Action<TParameter> initialize ) : this( factory, initialize, Delegates<TResult>.Empty ) {}

		public ConfiguringFactory( Func<TParameter, TResult> factory, Action<TParameter> initialize, Action<TResult> configure ) : base( factory )
		{
			this.initialize = initialize;
			this.configure = configure;
		}

		public override TResult Get( TParameter parameter )
		{
			initialize( parameter );
			var result = base.Get( parameter );
			configure( result );
			return result;
		}
	}
}