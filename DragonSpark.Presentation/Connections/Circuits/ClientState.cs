using DragonSpark.Compose;
using DragonSpark.Presentation.Components.State.Persistence;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Connections.Circuits;

public sealed class ClientState : Persist<string>
{
	public ClientState(PersistentComponentState state) : base(state, A.Type<ClientState>()) {}
}