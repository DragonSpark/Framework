using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Connections;

sealed class ServerRegistrations : ICommand<IServiceCollection>
{
	public static ServerRegistrations Default { get; } = new();

	ServerRegistrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<IsSigned>().Include(x => x.Dependencies).Singleton();
	}
}