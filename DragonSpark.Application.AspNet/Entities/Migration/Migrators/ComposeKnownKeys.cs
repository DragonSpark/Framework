using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class ComposeKnownKeys<T> : ISelect<DbContext, ImmutableHashSet<object>> where T : class
{
	public static ComposeKnownKeys<T> Default { get; } = new();

	ComposeKnownKeys() : this(typeof(T)) {}
	
	readonly Type _type;

	public ComposeKnownKeys(Type type) => _type = type;

	public ImmutableHashSet<object> Get(DbContext parameter)
	{
		var type       = parameter.Model.FindEntityType(_type).Verify();
		var key        = type.FindPrimaryKey().Verify();
		var properties = key.Properties;
		var source     = parameter.Set<T>().AsNoTracking();
		var name       = properties[0].Name;
		switch (properties.Count)
		{
			case 1:
				return source.Select(e => EF.Property<object>(e, name)).AsEnumerable().ToImmutableHashSet();
			default:
				return source
				       .Select(e => new
				       {
					       A = EF.Property<object>(e, name), B = EF.Property<object>(e, properties[1].Name)
				       })
				       .AsEnumerable()
				       .Select(static x => (object)ValueTuple.Create(x.A, x.B))
				       .ToImmutableHashSet();
		}
	}
}