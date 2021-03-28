using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	public class Add<TIn, TOut> : ISelecting<TIn, TOut>
	{
		readonly Await<TIn, TOut> _new;
		readonly IUpdate<TOut>    _update;

		public Add(ISelecting<TIn, TOut> @new, IUpdate<TOut> update) : this(@new.Await, update) {}

		public Add(Await<TIn, TOut> @new, IUpdate<TOut> update)
		{
			_new    = @new;
			_update = update;
		}

		public async ValueTask<TOut> Get(TIn parameter)
		{
			var result = await _new(parameter);
			await _update.Await(result);
			return result;
		}
	}
}