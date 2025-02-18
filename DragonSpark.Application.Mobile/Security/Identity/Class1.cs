using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Application.Security.Identity;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Text;
using Microsoft.Extensions.DependencyInjection;
using Uno.Extensions;
using Uno.Extensions.Authentication;
using Uno.Extensions.Specialized;

namespace DragonSpark.Application.Mobile.Security.Identity;

public sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<UpdateCurrentPrincipal>()
		         .Include(x => x.Dependencies.Recursive())
		         .Singleton()
		         .Then.AddSingleton<IPrincipalAccess, PrincipalAccess>()
		         .AddSingleton<ICurrentPrincipal, CurrentPrincipal>()
		         //
		         .Decorate<ITokenCache, CompensatingTokenCache>()
		         .Decorate<IAuthenticationService, CurrentPrincipalAwareAuthenticationService>()
			;
	}
}

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

sealed class CurrentPrincipalAwareAuthenticationService : IAuthenticationService
{
	readonly IAuthenticationService _previous;
	readonly ITokenCache            _token;
	readonly UpdateCurrentPrincipal _update;

	public CurrentPrincipalAwareAuthenticationService(IAuthenticationService previous, ITokenCache token,
	                                                  UpdateCurrentPrincipal update)
	{
		_previous = previous;
		_token    = token;
		_update   = update;
	}

	// ReSharper disable once TooManyArguments
	public async ValueTask<bool> LoginAsync(IDispatcher? dispatcher, IDictionary<string, string>? credentials = null,
	                                        string? provider = null, CancellationToken? cancellationToken = null)
	{
		var result = await _previous.LoginAsync(dispatcher, credentials, provider, cancellationToken).Await();
		await Update(cancellationToken, result).Await();
		return result;
	}

	async Task Update(CancellationToken? cancellationToken, bool result)
	{
		await _update.Await(await IsAuthenticated(cancellationToken).Await() && result);
	}

	public async ValueTask<bool> RefreshAsync(CancellationToken? cancellationToken = null)
	{
		var before = await _token.RefreshTokenAsync(cancellationToken).Await();
		var result = await _previous.RefreshAsync(cancellationToken).Await();
		var after  = await _token.RefreshTokenAsync(cancellationToken).Await();
		await Update(cancellationToken, result).Await();
		return result;
	}

	public async ValueTask<bool> LogoutAsync(IDispatcher? dispatcher, CancellationToken? cancellationToken = null)
	{
		var result = await _previous.LogoutAsync(dispatcher, cancellationToken).Await();
		await Update(cancellationToken, result).Await();
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

public readonly record struct IdentityPayload(string Access, string Identity);

sealed class PrincipalStores : ReferenceValueTable<string, PrincipalStore>, ICommand
{
	readonly ConditionalWeakTable<string, PrincipalStore> _store;
	public static PrincipalStores Default { get; } = new();

	public PrincipalStores() : this(new()) {}

	public PrincipalStores(ConditionalWeakTable<string, PrincipalStore> store) : base(x => new(x)) => _store = store;

	public void Execute(None parameter)
	{
		_store.Clear();
	}
}

sealed class UpdateCurrentPrincipal : IOperation<bool>
{
	readonly AssignCurrentPrincipal _assign;
	readonly ICommand               _clear;

	public UpdateCurrentPrincipal(AssignCurrentPrincipal assign, PrincipalStores clear)
	{
		_assign = assign;
		_clear  = clear;
	}

	public async ValueTask Get(bool parameter)
	{
		if (parameter)
		{
			await _assign.Await();
		}
		else
		{
			_clear.Execute();
		}
	}
}

sealed class AssignCurrentPrincipal : IOperation
{
	readonly ITokenCache      _tokens;
	readonly PrincipalStores  _stores;
	readonly IPrincipalAccess _store;

	public AssignCurrentPrincipal(ITokenCache tokens, IPrincipalAccess store)
		: this(tokens, PrincipalStores.Default, store) {}

	public AssignCurrentPrincipal(ITokenCache tokens, PrincipalStores stores, IPrincipalAccess store)
	{
		_tokens = tokens;
		_stores = stores;
		_store  = store;
	}

	public async ValueTask Get()
	{
		var access    = await _tokens.AccessTokenAsync().Await();
		var identity  = await _tokens.TokenAsync(TokenCacheExtensions.IdTokenKey).Await();
		var valid     = access.Account() is not null && identity.Account() is not null;
		var principal = valid ? _stores.Get(access).Get(identity) : AnonymousPrincipal.Default;
		_store.Execute(principal);
		if (!valid)
		{
			_stores.Execute();
		}
	}
}

public interface IPrincipalAccess : IMutable<ClaimsPrincipal?>;

sealed class AnonymousPrincipal : Instance<ClaimsPrincipal>
{
	public static AnonymousPrincipal Default { get; } = new();

	AnonymousPrincipal() : base(new(new ClaimsIdentity())) {}
}

sealed class CurrentPrincipal : ICurrentPrincipal
{
	readonly IPrincipalAccess _access;
	readonly ClaimsPrincipal  _default;

	public CurrentPrincipal(IPrincipalAccess access) : this(access, AnonymousPrincipal.Default) {}

	public CurrentPrincipal(IPrincipalAccess access, ClaimsPrincipal @default)
	{
		_access  = access;
		_default = @default;
	}

	public ClaimsPrincipal Get() => _access.Get() ?? _default;
}

sealed class PrincipalStore : ReferenceValueStore<string, ClaimsPrincipal>
{
	public PrincipalStore(string access) : this(access, CreatePrincipal.Default.Get) {}

	public PrincipalStore(string access, Func<IdentityPayload, ClaimsPrincipal> load)
		: base(x => load(new(access, x))) {}
}

sealed class CreatePrincipal : ISelect<IdentityPayload, ClaimsPrincipal>
{
	public static CreatePrincipal Default { get; } = new();

	CreatePrincipal() : this(CreateIdentity.Default) {}

	readonly ISelect<string, ClaimsIdentity> _identity;

	public CreatePrincipal(ISelect<string, ClaimsIdentity> identity) => _identity = identity;

	public ClaimsPrincipal Get(IdentityPayload parameter)
	{
		var (access, identity) = parameter;
		return new([_identity.Get(access), _identity.Get(identity)]);
	}
}

sealed class CreateIdentity : ISelect<string, ClaimsIdentity>
{
	public static CreateIdentity Default { get; } = new();

	CreateIdentity() : this(IdentityTokenParser.Default) {}

	readonly IParser<JwtSecurityToken> _token;

	public CreateIdentity(IParser<JwtSecurityToken> token) => _token = token;

	public ClaimsIdentity Get(string parameter) => new(_token.Get(parameter).Claims, JwtConstants.HeaderType);
}

sealed class PrincipalAccess : Variable<ClaimsPrincipal>, IPrincipalAccess;