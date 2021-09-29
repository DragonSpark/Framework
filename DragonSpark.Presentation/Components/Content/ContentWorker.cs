using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content
{
	public readonly record struct ContentWorker<T>(Task Worker, Task<T?> Result);

	public readonly record struct ContentWorker(Task Worker, Task Result);
}