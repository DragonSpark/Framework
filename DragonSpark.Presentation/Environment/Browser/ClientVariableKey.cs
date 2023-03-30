using DragonSpark.Compose;
using System;

namespace DragonSpark.Presentation.Environment.Browser;

public readonly record struct ClientVariableKey(Type Owner, string Identifier)
{
	public ClientVariableKey(object Owner, Guid Identifier) : this(Owner, Identifier.ToString()) {}

	public ClientVariableKey(object Owner, string Identifier) : this(Owner.GetType(), Identifier) {}

	public override string ToString() => $"{Owner.FullName.Verify()}+{Identifier}";
}