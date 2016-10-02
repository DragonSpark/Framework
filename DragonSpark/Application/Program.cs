namespace DragonSpark.Application
{
	public abstract class Program<T> : IProgram
	{
		void IProgram.Run( object arguments )
		{
			Run( (T)arguments );
		}

		protected abstract void Run( T arguments );
	}
}