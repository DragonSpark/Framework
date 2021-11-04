using DragonSpark.Application.Compose.Store;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity;

sealed class StateViewKey : Key<ClaimsPrincipal>
{
	public static StateViewKey Default { get; } = new();

	StateViewKey() : base(nameof(StateViewKey), x => x.UserName()) {}
}