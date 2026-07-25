namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

sealed class PropertyRecordEqualityComparer : IEqualityComparer<PropertyRecord>
{
	public static PropertyRecordEqualityComparer Default { get; } = new();

	PropertyRecordEqualityComparer() : this(EntityMetadataEqualityComparer.Default) {}

	readonly IEqualityComparer<Type> _type;

	public PropertyRecordEqualityComparer(IEqualityComparer<Type> type) => _type = type;

	public bool Equals(PropertyRecord x, PropertyRecord y) => x.Name == y.Name && _type.Equals(x.Type, y.Type);

	public int GetHashCode(PropertyRecord obj) => HashCode.Combine(obj.Name, _type.GetHashCode(obj.Type));
}