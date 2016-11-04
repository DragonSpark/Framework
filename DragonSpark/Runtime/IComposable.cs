namespace DragonSpark.Runtime
{
	public interface IComposable<in T>
	{
		void Add( T instance );
	}
}