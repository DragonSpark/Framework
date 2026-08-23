using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using DragonSpark.Presentation.Components.Content.Rendering;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class RenderStateAwareDataRequests : ISelect<RenderStateAwareDataRequestsInput, IDataRequest>
{
	readonly RenderCache       _memory;
	readonly IRenderState      _state;
	readonly IRenderContentKey _key;

	public RenderStateAwareDataRequests(RenderCache memory, IRenderState state, IRenderContentKey key)
	{
		_memory = memory;
		_state  = state;
		_key    = key;
	}

	public IDataRequest Get(RenderStateAwareDataRequestsInput parameter)
	{
		var (owner, source) = parameter;
		var key = _key.Get(owner);
		return new Selection(source, _state, new(_memory, key));
	}

	sealed class Selection : RenderAwareSelection<Stop<DataManagerRequest>, DataResult>, IDataRequest
	{
		public Selection(IDataRequest previous, IRenderState state, RenderVariable<DataResult> variable)
			: base(previous, state, variable) {}
	}
}