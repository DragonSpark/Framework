using DragonSpark.Compose;
using DragonSpark.Presentation.Environment.Browser;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace DragonSpark.SyncfusionRendering.Components;

public sealed class GridStateVariable : ClientVariable<string>
{
	public GridStateVariable(string identity, ProtectedSessionStorage storage)
		: base($"{A.Type<GridStateVariable>().GUID}+{identity}", storage) {}
}