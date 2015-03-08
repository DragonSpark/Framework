
namespace DragonSpark.Configuration
{
	public interface IInstanceSource<out TResult>
	{
		TResult Instance { get; }
	}
}