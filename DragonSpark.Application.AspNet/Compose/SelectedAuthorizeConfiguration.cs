using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.AspNet.Compose;

sealed class SelectedAuthorizeConfiguration<T> : ICommand<IServiceCollection> where T : class
{
	readonly Action<AuthorizationOptions, T> _command;

	public SelectedAuthorizeConfiguration(Action<AuthorizationOptions, T> command) => _command = command;

	public void Execute(IServiceCollection parameter)
	{
		parameter.AddOptions<AuthorizationOptions>().Configure(_command);
	}
}