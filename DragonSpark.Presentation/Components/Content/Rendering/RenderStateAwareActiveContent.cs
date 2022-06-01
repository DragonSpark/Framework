using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class RenderStateAwareActiveContent<T> : IActiveContent<T>
{
	readonly IResult<RenderState>        _state;
	readonly ISelecting<RenderState, T?> _content;

	public RenderStateAwareActiveContent(IUpdateMonitor monitor, IResult<RenderState> state,
	                                     ISelecting<RenderState, T?> content)
	{
		Monitor  = monitor;
		_state   = state;
		_content = content;
	}

	public async ValueTask<T?> Get()
	{
		var state  = _state.Get();
		var result = await _content.Await(state);
		return result;
	}

	public IUpdateMonitor Monitor { get; }
}