using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using JetBrains.Annotations;
using System;

namespace DragonSpark.Sources.Coercion
{
	public class ParameterCoercionSource<TParameter, TResult> : ParameterCoercionSource<object, TParameter, TResult>
	{
		readonly static IParameterizedSource<TParameter> Coercer = Coercer<TParameter>.Default;
		public ParameterCoercionSource( Func<TParameter, TResult> source ) : this( Coercer, source ) {}

		[UsedImplicitly]
		public ParameterCoercionSource( IParameterizedSource<object, TParameter> coercer, Func<TParameter, TResult> source ) : base( coercer.Get, source ) {}
	}

	public class ParameterCoercionSource<TFrom, TParameter, TResult> : DelegatedParameterizedSource<TParameter, TResult>, IParameterizedSource<TFrom, TResult>
	{
		readonly Func<TFrom, TParameter> coercer;

		public ParameterCoercionSource( Func<TFrom, TParameter> coercer, Func<TParameter, TResult> source ) : base( source )
		{
			this.coercer = coercer;
		}

		public TResult Get( TFrom parameter )
		{
			var to = coercer( parameter );
			var result = to.IsAssigned() ? Get( to ) : default(TResult);
			return result;
		}
	}

	public class ResultCoercionSource<TParameter, TResult, TTo> : DelegatedParameterizedSource<TParameter, TResult>, IParameterizedSource<TParameter, TTo>
	{
		readonly Func<TResult, TTo> coercer;

		public ResultCoercionSource( Func<TParameter, TResult> source, Func<TResult, TTo> coercer ) : base( source )
		{
			this.coercer = coercer;
		}

		public new TTo Get( TParameter parameter ) => coercer( base.Get( parameter ) );
	}
}