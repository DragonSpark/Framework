using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Profile
{
	sealed class Create<T> : ICreate<T> where T : IdentityUser
	{
		readonly ICreated<T> _created;
		readonly INew<T>     _new;

		public Create(INew<T> @new, ICreated<T> created)
		{
			_created = created;
			_new     = @new;
		}

		public async ValueTask<CreateUserResult<T>> Get(ExternalLoginInfo parameter)
		{
			var user   = await _new.Await(parameter);
			var call   = await _created.Await(new(parameter, user));
			var result = new CreateUserResult<T>(user, call);
			return result;
		}
	}

	sealed class LoggingAwareCreate<T> : ICreate<T> where T : IdentityUser
	{
		readonly ICreate<T>                     _previous;
		readonly ILogger<LoggingAwareCreate<T>> _logger;

		public LoggingAwareCreate(ICreate<T> previous, ILogger<LoggingAwareCreate<T>> logger)
		{
			_previous = previous;
			_logger   = logger;
		}

		public async ValueTask<CreateUserResult<T>> Get(ExternalLoginInfo parameter)
		{
			var result = await _previous.Await(parameter);
			if (result.Result.Succeeded)
			{
				var (user, _) = result;
				_logger.LogInformation("User {UserName} created an account using {Provider} having {Key}.",
				                       user.UserName, parameter.LoginProvider, parameter.ProviderKey);
			}

			return result;
		}
	}
}