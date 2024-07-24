using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using OpenAI.Images;

namespace DragonSpark.Azure.Ai;

sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Register<AiServicesConfiguration>()
		         .Start<OpenAIClient>()
		         .Use<AiServicesClient>()
		         .Singleton()
		         //
		         .Then.Start<ImageClient>()
		         .Use<ImageClients>()
		         .Singleton();
	}
}