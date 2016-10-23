using System;

namespace DragonSpark.Sources.Parameterized
{
	public sealed class SourceAdapter<TParameter, TResult> : ParameterizedSourceBase<TParameter, TResult>
	{
		readonly Func<TResult> factory;

		public SourceAdapter( Func<TResult> factory )
		{
			this.factory = factory;
		}

		public override TResult Get( TParameter parameter ) => factory();
	}
}