using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Model.Operations;
using DragonSpark.Presentation.Components.Content.Rendering;
using DragonSpark.Text;

namespace DragonSpark.Presentation.Components.Content.Sequences;

sealed class MemoryAwarePaging<T> : RenderStateAwareSelection<QueryInput, Current<T>>, IPaging<T>
{
	public MemoryAwarePaging(IFormatter<QueryInput> key, ISelecting<RenderStateInput<QueryInput>, Current<T>> content)
		: base(key, content) {}
}