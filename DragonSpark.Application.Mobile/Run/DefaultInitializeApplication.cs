using System.Threading.Tasks;

namespace DragonSpark.Application.Mobile.Run;

sealed class DefaultInitializeApplication : IInitializeApplication
{
	public static DefaultInitializeApplication Default { get; } = new();

	DefaultInitializeApplication() {}
	
	public ValueTask Get(Application parameter) => ValueTask.CompletedTask;
}