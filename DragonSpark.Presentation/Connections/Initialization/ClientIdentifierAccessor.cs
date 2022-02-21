using DragonSpark.Presentation.Components.State;
using System;

namespace DragonSpark.Presentation.Connections.Initialization;

public sealed class ClientIdentifierAccessor : ClientVariableAccessor<Guid?>
{
	public static ClientIdentifierAccessor Default { get; } = new();

	ClientIdentifierAccessor() : base(ConnectionIdentifierName.Default, x => Guid.Parse(x)) {}
}