using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Components.Authorization;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Initialization;

sealed class ProcessAuthenticationUpdate<T> : IProcessAuthenticationUpdate where T : IdentityUser
{
	readonly IProcessAuthenticationUpdate      _previous;
	readonly IProcessAuthentication<T>         _process;
	readonly IMutable<AuthenticationState<T>?> _current;

	public ProcessAuthenticationUpdate(IProcessAuthenticationUpdate previous, IProcessAuthentication<T> process)
		: this(previous, process, new Variable<AuthenticationState<T>>()) {}

	public ProcessAuthenticationUpdate(IProcessAuthenticationUpdate previous, IProcessAuthentication<T> process,
	                                   IMutable<AuthenticationState<T>?> current)
	{
		_previous = previous;
		_process  = process;
		_current  = current;
	}

	public async ValueTask Get(Task<AuthenticationState> parameter)
	{
		await _previous.Off(parameter);

		var current = _current.Get();
		if (await parameter.Off() is AuthenticationState<T> user
		    && user != current && (current is null
		                           || current.User.IsAuthenticated() != user.User.IsAuthenticated()
		                           || current.Profile?.SecurityStamp != user.Profile?.SecurityStamp))
		{
			await _process.Off(user);
		}
	}
}

sealed class ProcessAuthenticationUpdate : IProcessAuthenticationUpdate
{
	readonly AuthenticationStateSource _source;

	public ProcessAuthenticationUpdate(AuthenticationStateSource source) => _source = source;

	public ValueTask Get(Task<AuthenticationState> parameter) => _source.NotifyChangedAsync(parameter).ToOperation();
}