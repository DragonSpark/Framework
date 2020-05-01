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
	public sealed class ProfileView<T> where T : class
	{
		public static ProfileView<T> Default { get; } = new ProfileView<T>();

		ProfileView() : this(AuthenticationState<T>.Default, null) {}

		public ProfileView(AuthenticationState<T> profile, string hash)
		{
			Profile = profile;
			Hash    = hash;
		}

		public AuthenticationState<T> Profile { get; }

		public string Hash { get; }
	}

	public sealed class AuthenticationState<T> : AuthenticationState
	{
		public static AuthenticationState<T> Default { get; } = new AuthenticationState<T>();

		AuthenticationState() : this(new ClaimsPrincipal(new ClaimsIdentity()), default) {}

		public AuthenticationState(ClaimsPrincipal user, T profile) : base(user) => Profile = profile;

		public T Profile { get; }

		public void Deconstruct(out T profile, out ClaimsPrincipal principal)
		{
			profile = Profile;
			principal = User;
		}
	}

	public interface IProfileViews<T> : IOperationResult<ClaimsPrincipal, ProfileView<T>> where T : class {}

	[UsedImplicitly]
	sealed class ProfileViews<T> : IProfileViews<T> where T : class
	{
		readonly IServiceScopeFactory _scopes;

		[UsedImplicitly]
		public ProfileViews(IServiceScopeFactory scopes) => _scopes = scopes;

		public async ValueTask<ProfileView<T>> Get(ClaimsPrincipal parameter)
		{
			var scope = _scopes.CreateScope();
			try
			{
				var users = scope.ServiceProvider.GetRequiredService<UserManager<T>>();
				var user  = await users.GetUserAsync(parameter);
				var result = new ProfileView<T>(new AuthenticationState<T>(parameter, user),
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

	sealed class StoredProfileViews<T> : IProfileViews<T> where T : class
	{
		readonly IProfileViews<T> _previous;
		readonly IMemoryCache     _store;
		readonly string           _prefix;

		[UsedImplicitly]
		public StoredProfileViews(IProfileViews<T> previous, IMemoryCache store)
			: this(previous, store, A.Type<StoredProfileViews<T>>().AssemblyQualifiedName) {}

		[UsedImplicitly]
		public StoredProfileViews(IProfileViews<T> previous, IMemoryCache store, string prefix)
		{
			_previous = previous;
			_store    = store;
			_prefix   = prefix;
		}

		public async ValueTask<ProfileView<T>> Get(ClaimsPrincipal parameter)
		{
			// TODO: Encapsulate:
			var key = $"{_prefix}+{parameter.Identity.Name ?? "Anonymous"}";
			var result = _store.TryGetValue(key, out var stored)
				             ? stored.To<ProfileView<T>>()
				             : _store.Set(key, await _previous.Get(parameter).ConfigureAwait(false),
				                          TimeSpan.FromSeconds(10));
			return result;
		}
	}

	sealed class AnonymousAwareProfile<T> : IProfileViews<T> where T : class
	{
		readonly IProfileViews<T> _views;
		readonly ProfileView<T>   _default;

		[UsedImplicitly]
		public AnonymousAwareProfile(IProfileViews<T> views) : this(views, ProfileView<T>.Default) {}

		public AnonymousAwareProfile(IProfileViews<T> views, ProfileView<T> @default)
		{
			_views   = views;
			_default = @default;
		}

		public ValueTask<ProfileView<T>> Get(ClaimsPrincipal parameter)
			=> parameter.Identity.IsAuthenticated ? _views.Get(parameter) : _default.ToOperation();
	}

	public interface IAdapters : IAlteration<Task<AuthenticationState>> {}

	sealed class Adapters<T> : IAdapters where T : class
	{
		readonly IProfileViews<T> _views;

		[UsedImplicitly]
		public Adapters(IProfileViews<T> views) => _views = views;

		public async Task<AuthenticationState> Get(Task<AuthenticationState> parameter)
		{
			var previous = await parameter.ConfigureAwait(false);
			var view     = await _views.Get(previous.User);
			var result   = view.Profile;
			return result;
		}
	}

	public interface IAuthenticationValidation : IOperationResult<ClaimsPrincipal, bool> {}

	public sealed class AuthenticationValidation<T> : IAuthenticationValidation where T : class
	{
		readonly IProfileViews<T> _views;
		readonly string           _type;

		[UsedImplicitly]
		public AuthenticationValidation(IProfileViews<T> views, IOptions<IdentityOptions> options)
			: this(views, options.Value.ClaimsIdentity.SecurityStampClaimType) {}

		public AuthenticationValidation(IProfileViews<T> views, string type)
		{
			_views = views;
			_type  = type;
		}

		public async ValueTask<bool> Get(ClaimsPrincipal parameter)
		{
			var state  = await _views.Get(parameter).ConfigureAwait(false);
			var result = state.Hash is null || parameter.FindFirstValue(_type) == state.Hash;
			return result;
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