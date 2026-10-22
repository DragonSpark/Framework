using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Dynamic.Core;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Source;

sealed class Maximum<T> : ISelect<DbSet<T>, object> where T : class
{
	readonly string                      _column;
	readonly ITable<IEntityType, object> _store;

	public Maximum(string column) : this(column, EntityMaximums.Default) {}

	public Maximum(string column, ITable<IEntityType, object> store)
	{
		_column = column;
		_store  = store;
	}

	public object Get(DbSet<T> parameter)
	{
		lock (_store)
		{
			if (_store.TryGet(parameter.EntityType, out var result))
			{
				return result;
			}

			var account = parameter.Max(_column).Account() ?? 0;
			_store.Assign(parameter.EntityType, account);
			return account;
		}
	}
}