
namespace DragonSpark.Runtime
{
	public interface ISingleton<out TResult> // TODO: Remove.
	{
		TResult Instance { get; }
	}
}