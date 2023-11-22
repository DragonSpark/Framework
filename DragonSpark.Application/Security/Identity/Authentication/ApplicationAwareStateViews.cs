using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication;

sealed class ApplicationAwareStateViews<T> : IStateViews<T> where T : IdentityUser
{
	readonly IStateViews<T>              _previous;
	readonly ICondition<ClaimsPrincipal> _application;

	public ApplicationAwareStateViews(IStateViews<T> previous) : this(previous, IsApplicationPrincipal.Default) {}

	public ApplicationAwareStateViews(IStateViews<T> previous, ICondition<ClaimsPrincipal> application)
	{
		_previous    = previous;
		_application = application;
	}

	public ValueTask<StateView<T>> Get(ClaimsPrincipal parameter)
		=> _application.Get(parameter)
			   ? _previous.Get(parameter)
			   : new StateView<T>(new(parameter, null), null).ToOperation();
}