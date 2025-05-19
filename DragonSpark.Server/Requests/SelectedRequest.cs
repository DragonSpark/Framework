using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DragonSpark.Server.Requests;

sealed class SelectedRequest<TIn, TOut> : IRequesting<TIn> where TOut : class
{
	readonly IStopAware<TIn, TOut?> _selecting;

	public SelectedRequest(IStopAware<TIn, TOut?> selecting) => _selecting = selecting;

	public async ValueTask<IActionResult> Get(Request<TIn> parameter)
	{
		var (owner, (_, _, subject)) = parameter;

		var previous = await _selecting.Off(new(subject, owner.HttpContext.RequestAborted));
		var result   = previous is not null ? previous as IActionResult ?? owner.Ok(previous) : owner.NotFound();
		return result;
	}
}