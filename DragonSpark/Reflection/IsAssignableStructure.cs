using System;
using DragonSpark.Runtime;

namespace DragonSpark.Reflection;

public sealed class IsAssignableStructure : IsAssigned<Type, Type>
{
	public static IsAssignableStructure Default { get; } = new();

	IsAssignableStructure() : base(Nullable.GetUnderlyingType!) {}
}