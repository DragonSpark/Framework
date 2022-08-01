using DragonSpark.Model.Commands;
using LightInject;

namespace DragonSpark.Presentation.Environment;

sealed class Composing : ICommand<IServiceContainer>
{
	public static Composing Default { get; } = new();

	Composing() {}

	public void Execute(IServiceContainer parameter) {}
}