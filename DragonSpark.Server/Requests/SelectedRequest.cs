using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DragonSpark.Server.Requests
{
	sealed class SelectedRequest<TIn, TOut> : IRequesting<TIn> where TOut : class
	{
		readonly ISelecting<TIn, TOut?> _selecting;

		public SelectedRequest(ISelecting<TIn, TOut?> selecting) => _selecting = selecting;

		public async ValueTask<IActionResult> Get(Request<TIn> parameter)
		{
			var (owner, (_, _, subject)) = parameter;

			var previous = await _selecting.Await(subject);
			var result   = previous is not null ? previous as IActionResult ?? owner.Ok(previous) : owner.NotFound();
			return result;
		}
	}
}