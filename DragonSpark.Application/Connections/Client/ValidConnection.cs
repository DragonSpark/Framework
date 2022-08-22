using DragonSpark.Compose;
using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.SignalR.Client;

namespace DragonSpark.Application.Connections.Client;

sealed class ValidConnection : Condition
{
	public ValidConnection(IMutable<HubConnection?> store)
		: base(new IsAssigned<HubConnection>(store).Then().And(new LastConnection()).Get().Get) {}
}