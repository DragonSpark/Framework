namespace DragonSpark.Runtime
{
	public abstract class Program<TArguments> : IProgram
	{
		void IProgram.Run( object arguments )
		{
			Run( (TArguments)arguments );
		}

		protected abstract void Run( TArguments arguments );
	}
}