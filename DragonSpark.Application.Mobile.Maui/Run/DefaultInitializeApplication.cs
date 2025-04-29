using System.Threading.Tasks;
using Microsoft.Maui.Hosting;

namespace DragonSpark.Application.Mobile.Maui.Run;

sealed class DefaultInitializeApplication : IInitializeApplication
{
	public static DefaultInitializeApplication Default { get; } = new();

	DefaultInitializeApplication() {}
	
	public ValueTask Get(MauiApp parameter) => ValueTask.CompletedTask;
}