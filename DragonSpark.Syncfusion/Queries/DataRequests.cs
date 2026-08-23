using DragonSpark.SyncfusionRendering.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class DataRequests : IDataRequests
{
	readonly ProtectedSessionStorage _session;

	public DataRequests(ProtectedSessionStorage session) => _session = session;

	public IDataRequest Get(DataRequestsInput parameter)
	{
		var (_, identity, active, current) = parameter;
		return new StateAwareDataRequest(current, new GridStateVariable(identity, _session), active);
	}
}