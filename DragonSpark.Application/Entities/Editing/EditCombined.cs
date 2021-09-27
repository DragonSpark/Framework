using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Model.Sequences.Memory;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing
{
	public class EditCombined<TIn, TContext, T> : IEditMany<TIn, T> where TContext : DbContext
	{
		readonly IEdit<TIn, Leasing<T>> _first, _second;

		protected EditCombined(IContexts<TContext> context, IQuery<TIn, T> first, IQuery<TIn, T> second)
			: this(context.Then().Use(first).Edit.Lease(), context.Then().Use(second).Edit.Lease()) {}

		protected EditCombined(IEdit<TIn, Leasing<T>> first, IEdit<TIn, Leasing<T>> second)
		{
			_first  = first;
			_second = second;
		}

		public async ValueTask<ManyEdit<T>> Get(TIn parameter)
		{
			var (editor, first) = await _first.Await(parameter);
			var (_, second)     = await _second.Await(parameter);
			var combined = first.Then().Concat(second).Result();
			second.Dispose();
			return new(editor, combined);
		}
	}
}