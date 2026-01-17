using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

sealed class EntityTypeEqualityComparer : IEqualityComparer<IEntityType>
{
	public static EntityTypeEqualityComparer Default { get; } = new();

	EntityTypeEqualityComparer() : this(EntityMetadataEqualityComparer.Default) {}

	readonly IEqualityComparer<Type> _type;

	public EntityTypeEqualityComparer(IEqualityComparer<Type> type) => _type = type;

	public bool Equals(IEntityType? x, IEntityType? y)
		=> ReferenceEquals(x, y) ||
		   (x is not null && y is not null && x.Name == y.Name && _type.Equals(x.ClrType, y.ClrType));

	public int GetHashCode(IEntityType obj) => HashCode.Combine(obj.Name, _type.GetHashCode(obj.ClrType));
}