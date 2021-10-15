using DragonSpark.Application.Entities.Queries.Runtime;
using DragonSpark.Application.Entities.Queries.Runtime.Materialize;
using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Sequences;

sealed class Pagings<T> : AnyAwareSelect<PagingInput<T>, IPaging<T>>
{
	public Pagings(IPagers<T> previous)
		: this(previous,
		       Start.A.Selection<PagingInput<T>>().By.Calling(x => x.Queries).Select(Any.Default).Then()) {}

	public Pagings(IPagers<T> previous, Await<PagingInput<T>, bool> any)
		: base(previous.Then().Operation().Out(), any, EmptyPaging<T>.Default) {}

	sealed class Any : IDepending<IQueries<T>>
	{
		public static Any Default { get; } = new();

		Any() : this(DefaultAny<T>.Default) {}

		readonly IAny<T> _any;

		public Any(IAny<T> any) => _any = any;

		public async ValueTask<bool> Get(IQueries<T> parameter)
		{
			using var query  = await parameter.Await();
			var       result = await _any.Await(query.Subject);
			return result;
		}
	}
}