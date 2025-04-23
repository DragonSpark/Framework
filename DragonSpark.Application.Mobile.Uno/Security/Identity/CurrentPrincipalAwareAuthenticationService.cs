using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Compose;
using Uno.Extensions;
using Uno.Extensions.Authentication;

namespace DragonSpark.Application.Mobile.Uno.Security.Identity;

sealed class CurrentPrincipalAwareAuthenticationService : IAuthenticationService
{
	readonly IAuthenticationService _previous;
	readonly UpdateCurrentPrincipal _update;

	public CurrentPrincipalAwareAuthenticationService(IAuthenticationService previous, UpdateCurrentPrincipal update)
	{
		_previous = previous;
		_update   = update;
	}

	// ReSharper disable once TooManyArguments
	public async ValueTask<bool> LoginAsync(IDispatcher? dispatcher, IDictionary<string, string>? credentials = null,
	                                        string? provider = null, CancellationToken? cancellationToken = null)
	{
		var result = await _previous.LoginAsync(dispatcher, credentials, provider, cancellationToken).Off();
		await Update(cancellationToken, result).Off();
		return result;
	}

	async Task Update(CancellationToken? cancellationToken, bool result)
	{
		await _update.Off(await IsAuthenticated(cancellationToken).Off() && result);
	}

	public async ValueTask<bool> RefreshAsync(CancellationToken? cancellationToken = null)
	{
		var result = await _previous.RefreshAsync(cancellationToken).Off();
		await Update(cancellationToken, result).Off();
		return result;
	}

	public async ValueTask<bool> LogoutAsync(IDispatcher? dispatcher, CancellationToken? cancellationToken = null)
	{
		var result = await _previous.LogoutAsync(dispatcher, cancellationToken).Off();
		await Update(cancellationToken, result).Off();
		return result;
	}

	public ValueTask<bool> IsAuthenticated(CancellationToken? cancellationToken = null)
		=> _previous.IsAuthenticated(cancellationToken);

	public string[] Providers => _previous.Providers;

	public event EventHandler? LoggedOut
	{
		add => _previous.LoggedOut += value;
		remove => _previous.LoggedOut -= value;
	}
}