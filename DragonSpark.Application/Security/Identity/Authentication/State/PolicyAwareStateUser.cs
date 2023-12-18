using DragonSpark.Application.Entities.Diagnostics;
using DragonSpark.Diagnostics;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Authentication.State;

sealed class PolicyAwareStateUser<T> : PolicyAwareSelecting<ClaimsPrincipal, T?>, IStateUser<T> where T : IdentityUser
{
	public PolicyAwareStateUser(IStateUser<T> previous) : base(previous, DurableConnectionPolicy.Default.Get()) {}
}