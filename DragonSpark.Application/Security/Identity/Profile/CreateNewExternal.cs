using DragonSpark.Application.Entities.Transactions;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Profile
{
	sealed class CreateNewExternal<T> : ISelecting<ExternalLoginInfo, CreateUserResult<T>> where T : class
	{
		readonly ICreate<T> _create;
		readonly INew<T>    _new;

		public CreateNewExternal(INew<T> @new, ICreate<T> create)
		{
			_create = create;
			_new    = @new;
		}

		public async ValueTask<CreateUserResult<T>> Get(ExternalLoginInfo parameter)
		{
			var user   = await _new.Await(parameter);
			var result = await _create.Await(new(parameter, user));
			return new(user, result);
		}
	}

	sealed class CreateExternal<T> : Transacting<ExternalLoginInfo, CreateUserResult<T>>, ICreateExternal<T>
		where T : IdentityUser
	{
		public CreateExternal(CreateNewExternal<T> previous, DatabaseTransactions database)
			: base(previous, database) {}
	}

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