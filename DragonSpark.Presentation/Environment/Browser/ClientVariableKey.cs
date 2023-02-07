using DragonSpark.Compose;
using System;

namespace DragonSpark.Presentation.Environment.Browser;

public readonly record struct ClientVariableKey(Type Owner, Guid Identifier)
{
	public ClientVariableKey(object Owner, Guid Identifier) : this(Owner.GetType(), Identifier) {}

	public override string ToString() => $"{Owner.AssemblyQualifiedName.Verify()}+{Identifier}";
}