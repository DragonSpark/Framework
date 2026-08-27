using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Grok.Image;

sealed class LocalRegistrations : ICommand<IServiceCollection>
{
	public static LocalRegistrations Default { get; } = new();

	LocalRegistrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<IGenerateImage>().Forward<GenerateImage>().Singleton();
	}
}