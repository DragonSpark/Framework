using Azure.AI.ContentSafety;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Azure.Content;

sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Register<ContentSafetyConfiguration>()
		         //
		         .Start<ContentSafetyClient>()
		         .Use<ComposeContentSafetyClient>()
		         .Singleton()
		         //
		         .Then.Start<AnalyzeImage>()
		         .Singleton();
	}
}