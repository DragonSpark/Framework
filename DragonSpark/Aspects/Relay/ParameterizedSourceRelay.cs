using DragonSpark.Sources.Parameterized;

namespace DragonSpark.Aspects.Relay
{
	public sealed class ParameterizedSourceRelay<TParameter, TResult> : IParameterizedSourceRelay
	{
		readonly IParameterizedSource<TParameter, TResult> source;

		public ParameterizedSourceRelay( IParameterizedSource<TParameter, TResult> source )
		{
			this.source = source;
		}

		public object Get( object parameter ) => source.Get( (TParameter)parameter );
	}
}