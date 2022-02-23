using DragonSpark.Application.Entities.Queries.Runtime;
using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Presentation.Components.Content.Sequences;

sealed class Pagings<T> : AnyAwareSelect<PagingInput<T>, IPaging<T>>
{
	public Pagings(IPagers<T> previous, IDepending<IQueries<T>> any)
		: this(previous,
		       Start.A.Selection<PagingInput<T>>().By.Calling(x => x.Queries).Select(any).Then()) {}

	public Pagings(IPagers<T> previous, Await<PagingInput<T>, bool> any)
		: base(any, previous.Then().Operation().Out(), EmptyPaging<T>.Default) {}
}