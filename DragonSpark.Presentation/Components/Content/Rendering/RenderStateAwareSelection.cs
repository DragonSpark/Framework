using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Text;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Rendering;

// TODO
class RenderStateAwareSelection<TIn, TOut> : ISelecting<TIn, TOut>
{
	readonly IFormatter<TIn>                         _key;
	readonly ISelecting<RenderStateInput<TIn>, TOut> _content;

	protected RenderStateAwareSelection(IFormatter<TIn> key, ISelecting<RenderStateInput<TIn>, TOut> content)
	{
		_key        = key;
		_content    = content;
	}

	public async ValueTask<TOut> Get(TIn parameter)
	{
		var key   = _key.Get(parameter);
		var result = await _content.Await(new(parameter, key));
		return result;
	}
}