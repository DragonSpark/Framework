using AsyncUtilities;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;

namespace DragonSpark.Application.Entities
{
	sealed class ProtectedQueries<T> : IAlteration<IQueryable<T>> where T : class
	{
		public static ProtectedQueries<T> Default { get; } = new ProtectedQueries<T>();

		ProtectedQueries() : this(Start.A.Selection<IQueryable>()
		                               .By.Calling(x => x.Provider)
		                               .Select(QueryContext.Default)
		                               .Select(Locks.Default)) {}

		readonly Func<IQueryable, AsyncLock> _locks;

		public ProtectedQueries(AsyncLock @lock) : this(Start.A.Selection.Of.Any.By.Returning(@lock)) {}

		public ProtectedQueries(Func<IQueryable, AsyncLock> locks) => _locks = locks;

		public IQueryable<T> Get(IQueryable<T> parameter)
		{
			var @lock    = _locks(parameter);
			var provider = new QueryProvider(parameter.Provider.To<IAsyncQueryProvider>(), @lock);
			var result   = new ProtectedQuery<T>(parameter.Querying(), provider, @lock);
			return result;
		}
	}
}