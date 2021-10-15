using DragonSpark.Reflection;

namespace DragonSpark.Model.Sequences.Collections.Groups;

sealed class DeclaredGroupIndexes<T> : InstanceMetadata<T, InsertGroupElementAttribute, int>
{
	public static DeclaredGroupIndexes<T> Default { get; } = new DeclaredGroupIndexes<T>();

	DeclaredGroupIndexes() {}
}