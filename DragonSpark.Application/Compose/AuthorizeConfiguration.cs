using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Compose
{
	sealed class AuthorizeConfiguration : ICommand<IServiceCollection>
	{
		readonly Action<AuthorizationOptions> _command;

		public AuthorizeConfiguration(Action<AuthorizationOptions> command) => _command = command;

		public void Execute(IServiceCollection parameter)
		{
			parameter.Configure(_command);
		}
	}
}
