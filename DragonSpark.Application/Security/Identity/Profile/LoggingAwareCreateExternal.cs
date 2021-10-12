using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Profile
{
	sealed class LoggingAwareCreateExternal<T> : ICreateExternal<T> where T : IdentityUser
	{
		readonly ICreateExternal<T>                     _previous;
		readonly ILogger<LoggingAwareCreateExternal<T>> _logger;

		public LoggingAwareCreateExternal(ICreateExternal<T> previous, ILogger<LoggingAwareCreateExternal<T>> logger)
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