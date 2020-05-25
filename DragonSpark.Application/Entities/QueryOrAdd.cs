using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities {
	public class QueryOrAdd<TKey, TEntity> : ISelecting<TKey, TEntity> where TEntity : class
	{
		readonly DbContext                       _context;
		readonly ISelecting<TKey, TEntity> _query;
		readonly ISelect<TKey, TEntity>          _create;

		protected QueryOrAdd(DbContext context, ISelecting<TKey, TEntity> query,
		                     ISelect<TKey, TEntity> create)
		{
			_context = context;
			_query   = query;
			_create  = create;
		}

		public async ValueTask<TEntity> Get(TKey parameter)
		{
			var existing = await _query.Get(parameter);
			if (existing == null)
			{
				var result = _create.Get(parameter);
				_context.Set<TEntity>().Add(result);
				await _context.SaveChangesAsync();
				return result;
			}

			return existing;
		}
	}
}