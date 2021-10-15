using DragonSpark.Presentation.Components.State;
using Microsoft.AspNetCore.Http;
using System;

namespace DragonSpark.Presentation.Connections.Initialization;

public sealed class ClientIdentifierAccessor : ClientVariableAccessor<Guid?>
{
	public ClientIdentifierAccessor(IHttpContextAccessor accessor)
		: base(accessor, ConnectionIdentifierName.Default, x => Guid.Parse(x)) {}
}