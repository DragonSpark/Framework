using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Compose {
	sealed class AuthenticationContextCommand : ICommand<IServiceCollection>
	{
		readonly System.Action<AuthenticationBuilder> _command;

		public AuthenticationContextCommand(System.Action<AuthenticationBuilder> command) => _command = command;

		public void Execute(IServiceCollection parameter)
		{
			_command(parameter.AddAuthentication());
		}
	}
}