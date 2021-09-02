using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	sealed class Save<T> : ISave<T> where T : class
	{
		readonly DbContext _context;
		readonly DbSet<T>  _set;

		public Save(DbContext context) : this(context, context.Set<T>()) {}

		public Save(DbContext context, DbSet<T> set)
		{
			_context = context;
			_set     = set;
		}

		public async ValueTask Get(T parameter)
		{
			_set.Update(parameter);
			await _context.SaveChangesAsync().ConfigureAwait(false);
		}
	}

	public class Save<TContext, T> : Modify<TContext, T> where TContext : DbContext where T : class
	{
		public Save(ISaveContext<TContext> save) : base(save, x => x.Context.Set<T>().Update(x.Parameter)) {}
	}

	public class Save<TIn, TContext, T> : IOperation<TIn> where T : class where TContext : DbContext
	{
		readonly ISelecting<TIn, T> _selecting;
		readonly Action<T>          _configure;
		readonly Save<TContext, T>  _save;

		protected Save(ISelecting<TIn, T> selecting, Action<T> configure, Save<TContext, T> save)
		{
			_selecting = selecting;
			_configure = configure;
			_save      = save;
		}

		public async ValueTask Get(TIn parameter)
		{
			var subject = await _selecting.Await(parameter);
			_configure(subject);
			await _save.Await(subject);
		}
	}
}