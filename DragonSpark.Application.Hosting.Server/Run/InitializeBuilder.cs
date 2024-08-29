using DragonSpark.Model.Selection;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Hosting.Server.Run;

sealed class InitializeBuilder : ISelect<string[], ApplicationBuilder>
{
	public static InitializeBuilder Default { get; } = new();

	InitializeBuilder() {}

	public ApplicationBuilder Get(string[] parameter)
	{
		var result = new ApplicationBuilder(parameter);
		result.Services.AddSingleton(result);
		return result;
	}
}