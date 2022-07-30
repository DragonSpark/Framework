using DragonSpark.Model.Commands;
using DragonSpark.Presentation.Connections;
using LightInject;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Environment;

sealed class Composing : ICommand<IServiceContainer>
{
	public static Composing Default { get; } = new();

	Composing() {}

	public void Execute(IServiceContainer parameter)
	{
		parameter.Decorate<IHttpContextFactory, HttpContextFactory>()
		         .Decorate<IInitializeConnection, ContextAwareInitializeConnection>();
	}
}