using DragonSpark.Presentation;
using DragonSpark.Presentation.Components.Content.Rendering;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class RenderAwareDataRequests : IDataRequests
{
	readonly IDataRequests                _previous;
	readonly IRenderState                   _state;
	readonly RenderStateAwareDataRequests _requests;

	public RenderAwareDataRequests(IDataRequests previous, IRenderState state, RenderStateAwareDataRequests requests)
	{
		_previous = previous;
		_state  = state;
		_requests = requests;
	}

	public IDataRequest Get(DataRequestsInput parameter)
	{
		var (owner, _, _, _) = parameter;
		var previous = _previous.Get(parameter);
		return _state.IsLoading() ? _requests.Get(new(owner, previous)) : previous;
	}
}