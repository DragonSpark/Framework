using DragonSpark.Application.Entities.Queries.Runtime;
using DragonSpark.Model.Operations;
using DragonSpark.Presentation.Components.Content.Rendering;
using DragonSpark.Text;

namespace DragonSpark.Presentation.Components.Content.Sequences;

sealed class MemoryAwareAny<T> : RenderStateAwareSelection<IQueries<T>, bool>, IDepending<IQueries<T>>
{
	public MemoryAwareAny(string key, ISelecting<RenderStateInput<IQueries<T>>, bool> content)
		: this(new AnyFormatter<T>(key), content) {}

	public MemoryAwareAny(IFormatter<IQueries<T>> key, ISelecting<RenderStateInput<IQueries<T>>, bool> content)
		: base(key, content) {}
}