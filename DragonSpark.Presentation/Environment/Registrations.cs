using DragonSpark.Model.Commands;
using LightInject;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Environment;

sealed class Registrations : ICommand<IServiceContainer>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceContainer parameter)
	{
		parameter.Decorate<IHttpContextFactory, HttpContextFactory>();
	}
}