using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Compose
{
	sealed class AuthenticationContextCommand : ICommand<IServiceCollection>
	{
		readonly Action<AuthenticationBuilder> _command;
		readonly Action<AuthenticationOptions>?       _configure;

		public AuthenticationContextCommand(Action<AuthenticationBuilder> command,
		                                    Action<AuthenticationOptions>? configure = null)
		{
			_command   = command;
			_configure = configure;
		}

		public void Execute(IServiceCollection parameter)
		{
			_command(_configure != null ? parameter.AddAuthentication(_configure) : parameter.AddAuthentication());
		}
	}
}