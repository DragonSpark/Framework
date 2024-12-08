using DragonSpark.Application.AspNet;
using Microsoft.AspNetCore.Builder;

namespace DragonSpark.Application.Hosting.Server.Blazor.Environment;

public sealed class ApplicationConfiguration : IApplicationConfiguration
{
	public static ApplicationConfiguration Default { get; } = new();

	ApplicationConfiguration() : this("/Error") {}

	readonly string _handler;
	readonly bool   _createScope;

	public ApplicationConfiguration(string handler, bool createScope = true)
	{
		_handler          = handler;
		_createScope = createScope;
	}

	public void Execute(IApplicationBuilder parameter)
	{
		parameter.UseExceptionHandler(_handler, createScopeForErrors: _createScope)
		         .UseHsts(); // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	}
}