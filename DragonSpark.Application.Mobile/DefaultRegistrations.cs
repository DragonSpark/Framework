using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile;

sealed class DefaultRegistrations : ICommand<IServiceCollection>
{
	public static DefaultRegistrations Default { get; } = new();

	DefaultRegistrations() {}

	public void Execute(IServiceCollection parameter) {}
}
