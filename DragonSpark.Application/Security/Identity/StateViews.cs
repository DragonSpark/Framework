using DragonSpark.Compose;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	sealed class StateViews<T> : IStateViews<T> where T : class
	{
		readonly IServiceScopeFactory _scopes;

		[UsedImplicitly]
		public StateViews(IServiceScopeFactory scopes) => _scopes = scopes;

		public async ValueTask<StateView<T>> Get(ClaimsPrincipal parameter)
		{
			var scope = _scopes.CreateScope();
			try
			{
				var users = scope.ServiceProvider.GetRequiredService<UserManager<T>>();
				var user  = await users.GetUserAsync(parameter);
				var result = new StateView<T>(new AuthenticationState<T>(parameter, user),
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
}