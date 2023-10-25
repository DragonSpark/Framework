using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Presentation.Components.Content.Rendering;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Compose;

public sealed class OperationComposer<T> : Application.Compose.OperationComposer<T>
{
	public OperationComposer(ISelect<T, ValueTask> subject) : base(subject) {}

	public OperationComposer<T> Watching(IResult<RenderState> state)
		=> new(new ActiveRenderAwareOperation<T>(this.Out(), state));
}