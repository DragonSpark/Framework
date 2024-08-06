using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations;

public sealed class TokenFilterAttribute : ActionFilterAttribute
{
	readonly IMutable<CancellationToken> _store;

	public TokenFilterAttribute() : this(AmbientToken.Default) {}

	public TokenFilterAttribute(IMutable<CancellationToken> store) => _store = store;

	public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
	{
		using (_store.Assigned(context.HttpContext.RequestAborted))
		{
			await base.OnActionExecutionAsync(context, next).Await();
		}
	}
}