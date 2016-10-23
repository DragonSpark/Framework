using DragonSpark.TypeSystem;
using System;

namespace DragonSpark.Sources.Parameterized
{
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