using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Hosting.Server;

sealed class DefaultServiceConfiguration : ICommand<IServiceCollection>
{
	readonly Action<MvcOptions> _configure;

	public DefaultServiceConfiguration(Action<MvcOptions> configure) => _configure = configure;

	public void Execute(IServiceCollection parameter)
	{
		parameter.AddControllers(_configure);
	}
}