using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Runtime.Environment
{
	public sealed class StorageTypeDefinition : Variable<Type>
	{
		public static StorageTypeDefinition Default { get; } = new StorageTypeDefinition();

		StorageTypeDefinition() : base(typeof(Variable<>)) {}
	}
}