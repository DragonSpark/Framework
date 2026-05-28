using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Hosting.Server.Run;

sealed class InitializeBuilder : ISelect<string[], ApplicationBuilder>
{
	public static InitializeBuilder Default { get; } = new();

	InitializeBuilder() {}

	public ApplicationBuilder Get(string[] parameter)
	{
		var builder = WebApplication.CreateBuilder(parameter);
		var result  = new ApplicationBuilder(builder);
		result.Services.AddSingleton(builder).AddSingleton(result);
		return result;
	}
}