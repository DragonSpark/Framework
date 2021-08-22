using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	public class Add<TIn, TOut> : ISelecting<TIn, TOut>
	{
		readonly Await<TIn, TOut> _new;
		readonly Await<TOut>      _add;

		protected Add(ISelecting<TIn, TOut> @new, IOperation<TOut> add) : this(@new.Await, add.Await) {}

		protected Add(Await<TIn, TOut> @new, Await<TOut> add)
		{
			_new = @new;
			_add = add;
		}

		public async ValueTask<TOut> Get(TIn parameter)
		{
			var result = await _new(parameter);
			await _add(result);
			return result;
		}
	}

	/*
	public class Add<T, TIn, TOut> : Add<TIn, TOut> where T : DbContext where TOut : class
	{
		protected Add(ISelecting<TIn, TOut> @new, IDbContextFactory<T> contexts)
			: base(@new, new Save<TOut>(contexts.CreateDbContext())) {}
	}
*/
}