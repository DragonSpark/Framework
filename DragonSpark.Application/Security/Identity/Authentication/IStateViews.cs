using DragonSpark.Model.Operations;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Authentication
{
	public interface IStateViews<T> : ISelecting<ClaimsPrincipal, StateView<T>> where T : class {}
}