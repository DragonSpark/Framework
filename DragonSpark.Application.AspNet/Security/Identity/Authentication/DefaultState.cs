using Microsoft.AspNetCore.Components.Authorization;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication;

sealed class DefaultState<T> : DragonSpark.Model.Results.Instance<AuthenticationState>, IDefaultState
	where T : IdentityUser
{
	public static DefaultState<T> Default { get; } = new();

	DefaultState() : base(AuthenticationState<T>.Default) {}
}