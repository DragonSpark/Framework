using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection.Alterations;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	public sealed class ProfileState<T> where T : class
	{
		public static ProfileState<T> Default { get; } = new ProfileState<T>();

		ProfileState() : this(Profile<T>.Default, null) {}

		public ProfileState(Profile<T> profile, string hash)
		{
			Profile = profile;
			Hash    = hash;
		}

		public Profile<T> Profile { get; }

		public string Hash { get; }
	}

	public sealed class AuthenticationState<T> : AuthenticationState
	{
		public AuthenticationState(ClaimsPrincipal user, T profile) : base(user) => Profile = profile;

		public T Profile { get; }
	}

	public interface IProfileStates<T> : IOperationResult<ClaimsPrincipal, ProfileState<T>> where T : class {}

	sealed class ProfileStates<T> : IProfileStates<T> where T : class
	{
		readonly IServiceScopeFactory _scopes;

		public ProfileStates(IServiceScopeFactory scopes) => _scopes = scopes;

		public async ValueTask<ProfileState<T>> Get(ClaimsPrincipal parameter)
		{
			var scope = _scopes.CreateScope();
			try
			{
				var users = scope.ServiceProvider.GetRequiredService<UserManager<T>>();
				var user  = await users.GetUserAsync(parameter);
				var result = new ProfileState<T>(new Profile<T>(parameter, user),
				                                 users.SupportsUserSecurityStamp
					                                 ? await users.GetSecurityStampAsync(user) ?? string.Empty
					                                 : null);

				return result;
			}
			finally
			{
				await scope.Disposed();
			}
		}
	}

	sealed class AnonymousAwareProfile<T> : IProfileStates<T> where T : class
	{
		readonly IProfileStates<T> _states;
		readonly ProfileState<T>   _default;

		public AnonymousAwareProfile(IProfileStates<T> states) : this(states, ProfileState<T>.Default) {}

		public AnonymousAwareProfile(IProfileStates<T> states, ProfileState<T> @default)
		{
			_states  = states;
			_default = @default;
		}

		public ValueTask<ProfileState<T>> Get(ClaimsPrincipal parameter)
			=> parameter.Identity.IsAuthenticated ? _states.Get(parameter) : _default.ToOperation();
	}

	sealed class StoredProfileStates<T> : IProfileStates<T> where T : class
	{
		readonly IProfileStates<T> _previous;
		readonly IMemoryCache      _store;
		readonly string            _key;

		[UsedImplicitly]
		public StoredProfileStates(IProfileStates<T> previous, IMemoryCache store)
			: this(previous, store, A.Type<StoredProfileStates<T>>().AssemblyQualifiedName) {}

		[UsedImplicitly]
		public StoredProfileStates(IProfileStates<T> previous, IMemoryCache store, string key)
		{
			_previous = previous;
			_store    = store;
			_key      = key;
		}

		public async ValueTask<ProfileState<T>> Get(ClaimsPrincipal parameter)
		{
			// TODO: Encapsulate:
			var key = $"{_key}+{parameter.Identity.Name ?? "Anonymous"}";
			var result = _store.TryGetValue(key, out var stored)
				             ? stored.To<ProfileState<T>>()
				             : _store.Set(key, await _previous.Get(parameter).ConfigureAwait(false),
				                          TimeSpan.FromSeconds(10));
			return result;
		}
	}

	public interface IAdapters : IAlteration<Task<AuthenticationState>> {}

	sealed class Adapters<T> : IAdapters where T : class
	{
		readonly IProfileStates<T> _states;

		[UsedImplicitly]
		public Adapters(IProfileStates<T> states) => _states = states;

		public async Task<AuthenticationState> Get(Task<AuthenticationState> parameter)
		{
			var previous = await parameter.ConfigureAwait(false);
			var state    = await _states.Get(previous.User);
			var result   = new AuthenticationState<T>(previous.User, state.Profile.User);
			return result;
		}
	}

	public interface IAuthenticationValidation : IOperationResult<ClaimsPrincipal, bool> {}

	public sealed class AuthenticationValidation<T> : IAuthenticationValidation where T : class
	{
		readonly IProfileStates<T> _states;
		readonly string            _type;

		[UsedImplicitly]
		public AuthenticationValidation(IProfileStates<T> states, IOptions<IdentityOptions> options)
			: this(states, options.Value.ClaimsIdentity.SecurityStampClaimType) {}

		public AuthenticationValidation(IProfileStates<T> states, string type)
		{
			_states = states;
			_type   = type;
		}

		public async ValueTask<bool> Get(ClaimsPrincipal parameter)
		{
			var state  = await _states.Get(parameter).ConfigureAwait(false);
			var result = state.Hash is null || parameter.FindFirstValue(_type) == state.Hash;
			return false;
		}
	}

	public interface IValidationServices : IAdapters, IAuthenticationValidation {}

	sealed class ValidationServices : IValidationServices
	{
		readonly IAdapters                 _adapters;
		readonly IAuthenticationValidation _validation;

		[UsedImplicitly]
		public ValidationServices(IAdapters adapters, IAuthenticationValidation validation)
		{
			_adapters   = adapters;
			_validation = validation;
		}

		public Task<AuthenticationState> Get(Task<AuthenticationState> parameter) => _adapters.Get(parameter);

		public ValueTask<bool> Get(ClaimsPrincipal parameter) => _validation.Get(parameter);
	}
}