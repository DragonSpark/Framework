using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class KnownKeys<T> : ISelect<DbContext, ImmutableHashSet<object>> where T : class
{
	public static KnownKeys<T> Default { get; } = new();

	KnownKeys() {}

	public ImmutableHashSet<object> Get(DbContext parameter)
	{
		var entityType = parameter.Model.FindEntityType(A.Type<T>()).Verify();
		var key        = entityType.FindPrimaryKey().Verify();
		var names      = string.Join(',', key.Properties.Select(x => x.Name));
		var result     = parameter.Set<T>().Select(names).Cast<object>().ToImmutableHashSet();
		return result;
	}
}