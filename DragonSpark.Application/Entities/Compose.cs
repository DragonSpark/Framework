using DragonSpark.Model.Commands;
using LightInject;

namespace DragonSpark.Application.Entities;

sealed class Compose : ICommand<IServiceContainer>
{
	public static Compose Default { get; } = new Compose();

	Compose() {}

	public void Execute(IServiceContainer parameter) {}
}