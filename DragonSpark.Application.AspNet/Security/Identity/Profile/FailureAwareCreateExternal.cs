using DragonSpark.Compose;
using DragonSpark.Diagnostics.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NetFabric.Hyperlinq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Profile;

sealed class FailureAwareCreateExternal<T> : ICreateExternal<T> where T : IdentityUser
{
	readonly ICreateExternal<T> _previous;
	readonly Warning            _warning;

	public FailureAwareCreateExternal(ICreateExternal<T> previous, Warning warning)
	{
		_previous = previous;
		_warning  = warning;
	}

	public async ValueTask<CreateUserResult<T>> Get(ExternalLoginInfo parameter)
	{
		var result = await _previous.Await(parameter);
		if (!result.Result.Succeeded)
		{
			_warning.Execute(parameter.LoginProvider, parameter.ProviderKey,
			                 result.Result.Errors.AsValueEnumerable()
			                       .Select(x => $"{x.Code}: {x.Description}")
			                       .ToArray());
		}

		return result;
	}

	internal sealed class Warning : LogWarning<string, string, string[]>
	{
		public Warning(ILogger<Warning> logger)
			: base(logger, "Was not able to create using via {Provider} having {Key}. {Messages}") {}
	}
}