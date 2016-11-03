using System;

namespace DragonSpark.Sources.Parameterized
{
	public class DelegatedParameterizedSource<TParameter, TResult> : ParameterizedSourceBase<TParameter, TResult>
	{
		readonly Func<TParameter, TResult> second;

		public DelegatedParameterizedSource( Func<TParameter, TResult> second )
		{
			this.second = second;
		}

		public override TResult Get( TParameter parameter ) => second( parameter );
	}
}