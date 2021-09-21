using DragonSpark.Application.Entities.Queries.Runtime;
using DragonSpark.Application.Entities.Queries.Runtime.Materialize;
using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content
{
	class Class2 {}

	public sealed record RefreshQueriesMessage<T>(IQueries<T> Subject);

	public class AnyAwareSelect<TIn, TOut> : ISelecting<TIn, TOut>
	{
		readonly ISelecting<TIn, TOut> _previous;
		readonly Await<TIn, bool>      _any;
		readonly TOut                  _default;

		protected AnyAwareSelect(ISelecting<TIn, TOut> previous, Await<TIn, bool> any, TOut @default)
		{
			_previous = previous;
			_any      = any;
			_default  = @default;
		}

		public async ValueTask<TOut> Get(TIn parameter)
		{
			var any    = await _any(parameter);
			var result = any ? await _previous.Await(parameter) : _default;
			return result;
		}
	}

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
}