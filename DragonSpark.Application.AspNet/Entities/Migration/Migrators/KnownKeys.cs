using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class KnownKeys<T> : ISelect<DbContext, ImmutableHashSet<object>> where T : class
{
	public static KnownKeys<T> Default { get; } = new();

	KnownKeys() {}

	public ImmutableHashSet<object> Get(DbContext parameter)
	{
		var type       = parameter.Model.FindEntityType(typeof(T)).Verify();
		var key        = type.FindPrimaryKey().Verify();
		var properties = key.Properties;

		var source = parameter.Set<T>().AsNoTracking();
		switch (properties.Count)
		{
			case 1:
			{
				var name = properties[0].Name;
				return source.Select(e => EF.Property<object>(e, name)).AsEnumerable().ToImmutableHashSet();
			}
			default:
				return source
				       .Select(e => new
				       {
					       A = EF.Property<object>(e, properties[0].Name),
					       B = EF.Property<object>(e, properties[1].Name),
				       })
				       .AsEnumerable()
				       .Select(x => (object)new[] { x.A, x.B })
				       .ToImmutableHashSet();
		}
	}
}