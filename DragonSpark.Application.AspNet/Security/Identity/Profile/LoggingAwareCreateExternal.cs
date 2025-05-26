using DragonSpark.Compose;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Security.Identity.Profile;

sealed class LoggingAwareCreateExternal<T> : ICreateExternal<T> where T : IdentityUser
{
	readonly ICreateExternal<T> _previous;
	readonly Creating           _creating;
	readonly Created            _created;

	public LoggingAwareCreateExternal(ICreateExternal<T> previous, Creating creating, Created created)
	{
		_previous = previous;
		_creating = creating;
		_created  = created;
	}

	public async ValueTask<CreateUserResult<T>> Get(Stop<ExternalLoginInfo> parameter)
	{
		var (subject, _) = parameter;
		_creating.Execute(subject.LoginProvider, subject.ProviderKey);
		var result = await _previous.Off(parameter);
		if (result.Result.Succeeded)
		{
			var (user, _) = result;
			_created.Execute(user.UserName.Verify(), subject.LoginProvider, subject.ProviderKey);
		}

		return result;
	}

	internal sealed class Creating : LogMessage<string, string>
	{
		public Creating(ILogger<Creating> logger) : base(logger, "Creating new user using {Provider} having {Key}.") {}
	}

	internal sealed class Created : LogMessage<string, string, string>
	{
		public Created(ILogger<Created> logger) : base(logger, "Created {UserName} via {Provider} having {Key}.") {}
	}
}