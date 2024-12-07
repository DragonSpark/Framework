using DragonSpark.Model.Commands;
using LightInject;

namespace DragonSpark.Application.Security.Identity.Authentication;

sealed class Compose : ICommand<IServiceContainer>
{
	public static Compose Default { get; } = new();

	Compose() {}

	public void Execute(IServiceContainer parameter)
	{
		parameter.Decorate(typeof(IAuthentications<>), typeof(AmbientAwareAuthentications<>));
	}
}