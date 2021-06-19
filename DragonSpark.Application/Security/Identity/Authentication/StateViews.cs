using DragonSpark.Compose;
using DragonSpark.Composition;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication
{
	sealed class StateViews<T> : IStateViews<T> where T : class
	{
		readonly IServiceScopeFactory _scopes;
		readonly StateView<T>         _default;

		[UsedImplicitly]
		public StateViews(IServiceScopeFactory scopes) : this(scopes, StateView<T>.Default) {}

		[Candidate(false)]
		public StateViews(IServiceScopeFactory scopes, StateView<T> @default)
		{
			_scopes  = scopes;
			_default = @default;
		}

		public async ValueTask<StateView<T>> Get(ClaimsPrincipal parameter)
		{
			var scope = _scopes.CreateScope();
			try
			{
				var users = scope.ServiceProvider.GetRequiredService<UserManager<T>>();
				var user  = await users.GetUserAsync(parameter);
				var result = user != null
					             ? new StateView<T>(new AuthenticationState<T>(parameter, user),
					                                users.SupportsUserSecurityStamp
						                                ? await users.GetSecurityStampAsync(user) ?? string.Empty
						                                : null)
					             : _default;

				return result;
			}
			finally
			{
				await scope.Disposed();
			}
		}
	}
}