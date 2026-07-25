using DragonSpark.Model.Results;

namespace DragonSpark.Runtime.Environment;

public sealed class StorageTypeDefinition : Variable<Type>
{
	public static StorageTypeDefinition Default { get; } = new();

	StorageTypeDefinition() : base(typeof(Variable<>)) {}
}