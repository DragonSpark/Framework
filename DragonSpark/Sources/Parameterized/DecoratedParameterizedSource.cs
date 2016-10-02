namespace DragonSpark.Sources.Parameterized
{
	public class DecoratedParameterizedSource<TParameter, TResult> : DelegatedParameterizedSource<TParameter, TResult>
	{
		public DecoratedParameterizedSource( IParameterizedSource<TParameter, TResult> source ) : base( source.Get ) {}
	}
}