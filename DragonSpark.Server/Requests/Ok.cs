using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DragonSpark.Server.Requests
{
	public sealed class Ok<T> : IRequesting<T>
	{
		readonly ISelect<T, ValueTask> _selecting;

		public Ok(ISelect<T, ValueTask> selecting) => _selecting = selecting;

		public async ValueTask<IActionResult> Get(Request<T> parameter)
		{
			var (owner, (_, _, subject)) = parameter;
			await _selecting.Await(subject);
			return owner.Ok();
		}
	}
}