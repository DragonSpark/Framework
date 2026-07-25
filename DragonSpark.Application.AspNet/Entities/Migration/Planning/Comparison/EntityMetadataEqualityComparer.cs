using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

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