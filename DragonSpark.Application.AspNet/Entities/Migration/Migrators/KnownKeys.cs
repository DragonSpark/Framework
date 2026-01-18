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
		var entityType = parameter.Model.FindEntityType(typeof(T)).Verify();
		var key        = entityType.FindPrimaryKey().Verify();
		var props      = key.Properties;

		switch (props.Count)
		{
			case 1:
			{
				var name = props[0].Name;
				return parameter.Set<T>().Select(e => EF.Property<object>(e, name)).AsEnumerable().ToImmutableHashSet();
			}
			default:
				return parameter.Set<T>()
				                .Select(e => new
				                {
					                A = EF.Property<object>(e, props[0].Name),
					                B = EF.Property<object>(e, props[1].Name),
				                })
				                .AsEnumerable()
				                .Select(x => (object)new[] { x.A, x.B })
				                .ToImmutableHashSet();
		}
	}
}