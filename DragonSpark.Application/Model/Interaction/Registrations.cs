using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Model.Interaction;

sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter) {}
}