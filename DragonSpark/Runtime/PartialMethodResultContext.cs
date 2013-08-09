namespace DragonSpark.Runtime
{
	public class PartialMethodResultContext<TResult>
	{
		public TResult Result { get; set; }

		public PartialMethodResultContext() : this( default( TResult ) )
		{}

		public PartialMethodResultContext( TResult result )
		{
			Result = result;
		}
	}
}