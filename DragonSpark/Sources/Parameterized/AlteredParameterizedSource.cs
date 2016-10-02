using System;

namespace DragonSpark.Sources.Parameterized
{
	public sealed class AlteredParameterizedSource<TParameter, TResult> : DelegatedParameterizedSource<TParameter, TResult>
	{
		readonly Alter<TParameter> selector;

		public AlteredParameterizedSource( Alter<TParameter> selector, Func<TParameter, TResult> inner ) : base( inner )
		{
			this.selector = selector;
		}
		
		public override TResult Get( TParameter parameter ) => base.Get( selector( parameter ) );
	}
}