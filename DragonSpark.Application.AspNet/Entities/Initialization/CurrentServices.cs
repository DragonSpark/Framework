using DragonSpark.Runtime.Execution;

namespace DragonSpark.Application.AspNet.Entities.Initialization;

sealed class CurrentServices : Logical<IServiceProvider>
{
	public static CurrentServices Default { get; } = new();

	CurrentServices() {}
}