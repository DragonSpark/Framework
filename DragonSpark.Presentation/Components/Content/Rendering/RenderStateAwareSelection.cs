using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Text;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Rendering;

class RenderStateAwareSelection<TIn, TOut> : ISelecting<TIn, TOut>
{
	readonly IFormatter<TIn>                         _key;
	readonly RenderStates                            _store;
	readonly ISelecting<RenderStateInput<TIn>, TOut> _content;

	protected RenderStateAwareSelection(IFormatter<TIn> key, RenderStates store,
	                                    ISelecting<RenderStateInput<TIn>, TOut> content)
	{
		_key     = key;
		_store   = store;
		_content = content;
	}

	public async ValueTask<TOut> Get(TIn parameter)
	{
		var key   = _key.Get(parameter);
		var state = _store.Get(key);
		switch (state)
		{
			case RenderState.Default:
				_store.Assign(key, RenderState.Stored);
				break;
			case RenderState.Stored:
				_store.Remove(key);
				break;
		}

		var result = await _content.Await(new(parameter, key, state));
		return result;
	}
}