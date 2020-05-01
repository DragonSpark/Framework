using DragonSpark.Model.Operations;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity
{
	public interface IStateViews<T> : IOperationResult<ClaimsPrincipal, StateView<T>> where T : class {}
}