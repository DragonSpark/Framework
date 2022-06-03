using DragonSpark.Application.Entities.Diagnostics;
using DragonSpark.Diagnostics;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Authentication;

sealed class PolicyAwareStateViews<T> : PolicyAwareSelecting<ClaimsPrincipal, StateView<T>>, IStateViews<T>
	where T : IdentityUser
{
	public PolicyAwareStateViews(IStateViews<T> previous) : base(previous, DurableConnectionPolicy.Default.Get()) {}
}