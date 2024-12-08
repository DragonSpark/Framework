using DragonSpark.Model.Commands;
using LightInject;

namespace DragonSpark.Application.AspNet.Entities;

sealed class Compose : ICommand<IServiceContainer>
{
	public static Compose Default { get; } = new();

	Compose() {}

	public void Execute(IServiceContainer parameter) {}
}