namespace DragonSpark.Sources.Parameterized.Caching
{
	public interface IStackSource<T> : ISource<IStack<T>>
	{
		T GetCurrentItem();
	}
}