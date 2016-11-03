using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using JetBrains.Annotations;
using System;

namespace DragonSpark.Coercion
{
	public class LinkedParameterizedSource<TParameter, TResult> : LinkedParameterizedSource<object, TParameter, TResult>
	{
		readonly static IParameterizedSource<TParameter> Coercer = Coercer<TParameter>.Default;
		public LinkedParameterizedSource( Func<TParameter, TResult> source ) : this( Coercer, source ) {}

		[UsedImplicitly]
		public LinkedParameterizedSource( IParameterizedSource<object, TParameter> coercer, Func<TParameter, TResult> second ) : base( coercer.Get, second ) {}
	}

	public class LinkedParameterizedSource<TFrom, TParameter, TResult> : DelegatedParameterizedSource<TParameter, TResult>, IParameterizedSource<TFrom, TResult>
	{
		readonly Func<TFrom, TParameter> first;

		public LinkedParameterizedSource( Func<TFrom, TParameter> first, Func<TParameter, TResult> second ) : base( second )
		{
			this.first = first;
		}

		public TResult Get( TFrom parameter )
		{
			var to = first( parameter );
			var result = to.IsAssigned() ? Get( to ) : default(TResult);
			return result;
		}
	}
}