using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Compose;
using Uno.Extensions.Authentication;

namespace DragonSpark.Application.Mobile.Security.Identity;

sealed class CompensatingTokenCache : ITokenCache
{
	readonly ITokenCache _previous;

	public CompensatingTokenCache(ITokenCache previous) => _previous = previous;

	public ValueTask<string?> GetCurrentProviderAsync(CancellationToken ct) => _previous.GetCurrentProviderAsync(ct);

	public ValueTask<bool> HasTokenAsync(CancellationToken cancellationToken)
		=> _previous.HasTokenAsync(cancellationToken);

	public ValueTask<IDictionary<string, string>> GetAsync(CancellationToken cancellationToken)
		=> _previous.GetAsync(cancellationToken);

	public async ValueTask SaveAsync(string provider, IDictionary<string, string>? tokens,
	                                 CancellationToken cancellationToken)
	{
		var compensated = tokens?.Where(x => x.Value.Account() is not null).ToDictionary(x => x.Key, x => x.Value);
		await _previous.SaveAsync(provider, compensated, cancellationToken).Await();
	}

	public ValueTask ClearAsync(CancellationToken cancellationToken) => _previous.ClearAsync(cancellationToken);

	public event EventHandler? Cleared
	{
		add => _previous.Cleared += value;
		remove => _previous.Cleared -= value;
	}
}