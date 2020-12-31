using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DragonSpark.Server.Requests
{
	sealed class Requested<TIn, TOut> : IRequested<TIn> where TOut : class
	{
		readonly ISelecting<TIn, TOut?> _selecting;

		public Requested(ISelecting<TIn, TOut?> selecting) => _selecting = selecting;

		public async ValueTask<IActionResult> Get(Request<TIn> parameter)
		{
			var (owner, (_, _, subject)) = parameter;

			var previous = await _selecting.Await(subject);
			var result   = previous is not null ? previous as IActionResult ?? owner.Ok(previous) : owner.NotFound();
			return result;
		}
	}

	sealed class Requested<T> : IRequested<T>
	{
		readonly ISelect<T, ValueTask> _selecting;

		public Requested(ISelect<T, ValueTask> selecting) => _selecting = selecting;

		public async ValueTask<IActionResult> Get(Request<T> parameter)
		{
			var (owner, (_, _, subject)) = parameter;
			await _selecting.Await(subject);
			return owner.Ok();
		}
	}
}