using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities
{
	/*
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
	*/

	public class Add<TIn, TContext, TOut> : ConfiguringResult<TIn, TOut> where TContext : DbContext where TOut : class
	{
		protected Add(ISelecting<TIn, TOut> @new, Save<TContext, TOut> add) : base(@new, add) {}

		public Add(ISelecting<TIn, TOut> select, IOperation<TOut> operation) : base(select, operation) {}

		protected Add(Await<TIn, TOut> @new, Await<TOut> add) : base(@new, add) {}
	}
}