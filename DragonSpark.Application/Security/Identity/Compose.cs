using DragonSpark.Model.Commands;
using LightInject;

namespace DragonSpark.Application.Security.Identity;

sealed class Compose : ICommand<IServiceContainer>
{
	public static Compose Default { get; } = new();

	Compose() {}

	public void Execute(IServiceContainer parameter)
	{
		parameter.Decorate(typeof(IUsers<>), typeof(AmbientAwareUsers<>));
	}
}