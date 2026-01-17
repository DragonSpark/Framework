using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

public readonly record struct ComparePropertiesInput(PropertyDefinition From, PropertyDefinition To);

sealed class CompareProperties : ISelect<ComparePropertiesInput, PropertyComparison>
{
	public static CompareProperties Default { get; } = new();

	public CompareProperties()
		: this(PropertyRecordEqualityComparer.Default, EntityMetadataEqualityComparer.Default) {}

	readonly IEqualityComparer<PropertyRecord> _comparer;
	readonly IEqualityComparer<Type>           _types;

	public CompareProperties(IEqualityComparer<PropertyRecord> comparer, IEqualityComparer<Type> types)
	{
		_comparer = comparer;
		_types    = types;
	}

	public PropertyComparison Get(ComparePropertiesInput parameter)
	{
		var (from, to) = parameter;
		var added   = to.Set.Except(from.Set, _comparer).ToArray();
		var removed = from.Set.Except(to.Set, _comparer).ToArray();
		var changed = from.Set.Intersect(to.Set, _comparer)
		                  .Where(x => !_types.Equals(from.Map[x.Name].Type, to.Map[x.Name].Type))
		                  .ToArray();
		return new(added, removed, changed);
	}
}

sealed class PropertyRecordEqualityComparer : IEqualityComparer<PropertyRecord>
{
	public static PropertyRecordEqualityComparer Default { get; } = new();

	PropertyRecordEqualityComparer() : this(EntityMetadataEqualityComparer.Default) {}

	readonly IEqualityComparer<Type> _type;

	public PropertyRecordEqualityComparer(IEqualityComparer<Type> type) => _type = type;

	public bool Equals(PropertyRecord x, PropertyRecord y) => x.Name == y.Name && _type.Equals(x.Type, y.Type);

	public int GetHashCode(PropertyRecord obj) => HashCode.Combine(obj.Name, _type.GetHashCode(obj.Type));
}

sealed class LocationAwareEntityTypeEqualityComparer : IEqualityComparer<IEntityType>
{
	readonly IEntityTypes                   _types;
	readonly IModel                         _model;
	readonly IEqualityComparer<IEntityType> _previous;

	public LocationAwareEntityTypeEqualityComparer(IEntityTypes types)
		: this(types, types.Get(), EntityTypeEqualityComparer.Default) {}

	public LocationAwareEntityTypeEqualityComparer(IEntityTypes types, IModel model,
	                                               IEqualityComparer<IEntityType> previous)
	{
		_types    = types;
		_model    = model;
		_previous = previous;
	}

	public bool Equals(IEntityType? x, IEntityType? y)
	{
		var first  = x is not null && x.Model != _model ? _types.Get(x) : x;
		var second = y is not null && y.Model != _model ? _types.Get(y) : y;
		return first is not null && second is not null && _previous.Equals(first, second);
	}

	public int GetHashCode(IEntityType obj)
	{
		var type = obj.Model != _model ? _types.Get(obj) ?? obj : obj;
		return _previous.GetHashCode(type);
	}
}

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

sealed class EntityMetadataEqualityComparer : IEqualityComparer<Type>
{
	public static EntityMetadataEqualityComparer Default { get; } = new();

	EntityMetadataEqualityComparer() : this(DetermineMetadata.Default) {}

	readonly IAlteration<Type> _type;

	public EntityMetadataEqualityComparer(IAlteration<Type> type) => _type = type;

	public bool Equals(Type? x, Type? y)
	{
		var first  = x is not null ? _type.Get(x) : x;
		var second = y is not null ? _type.Get(y) : y;
		return first == second;
	}

	public int GetHashCode(Type obj) => _type.Get(obj).GetHashCode();
}

sealed class DetermineMetadata : IAlteration<Type>
{
	public static DetermineMetadata Default { get; } = new();

	DetermineMetadata() : this(typeof(Enum)) {}

	readonly Type _type;

	public DetermineMetadata(Type type) => _type = type;

	public Type Get(Type parameter) => parameter.IsEnum ? _type : parameter;
}