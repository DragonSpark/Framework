using DragonSpark.Compose;
using DragonSpark.Model.Selection;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class RenderAwareActiveContentBuilder<T> : ISelect<RenderAwareActiveContentBuilderInput<T>, IActiveContent<T>>
{
	readonly IRenderContentKey                 _key;
	readonly RenderStateAwareActiveContents<T> _contents;

	public RenderAwareActiveContentBuilder(IRenderContentKey key, RenderStateAwareActiveContents<T> contents)
	{
		_key      = key;
		_contents = contents;
	}

	public IActiveContent<T> Get(RenderAwareActiveContentBuilderInput<T> parameter)
	{
		var (previous, content) = parameter;
		var key    = _key.Get(content.Target.Verify());
		var input  = new RenderStateContentInput<T>(previous, key);
		var result = _contents.Get(input);
		return result;
	}
}