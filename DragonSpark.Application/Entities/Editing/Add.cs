using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities.Editing
{
	public class Add<TIn, TContext, TOut> : ConfiguringResult<TIn, TOut> where TContext : DbContext where TOut : class
	{
		protected Add(ISelecting<TIn, TOut> @new, Save<TContext, TOut> add) : base(@new, add) {}

		protected Add(ISelecting<TIn, TOut> select, IOperation<TOut> operation) : base(select, operation) {}

		protected Add(Await<TIn, TOut> @new, Await<TOut> add) : base(@new, add) {}
	}
}