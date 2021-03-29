using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	public class Add<TIn, TOut> : ISelecting<TIn, TOut>
	{
		readonly Await<TIn, TOut> _new;
		readonly ISave<TOut>      _save;

		public Add(ISelecting<TIn, TOut> @new, ISave<TOut> save) : this(@new.Await, save) {}

		public Add(Await<TIn, TOut> @new, ISave<TOut> save)
		{
			_new    = @new;
			_save = save;
		}

		public async ValueTask<TOut> Get(TIn parameter)
		{
			var result = await _new(parameter);
			await _save.Await(result);
			return result;
		}
	}
}