using System;

namespace DragonSpark.Sources.Parameterized
{
	public sealed class Wrapper<TParameter, TResult> : ParameterizedSourceBase<TParameter, TResult>
	{
		readonly Func<TResult> factory;

		public Wrapper( Func<TResult> factory )
		{
			this.factory = factory;
		}

		public override TResult Get( TParameter parameter ) => factory();
	}
}