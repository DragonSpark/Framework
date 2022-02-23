using DragonSpark.Application.Entities.Queries.Runtime;
using DragonSpark.Application.Entities.Queries.Runtime.Materialize;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using DragonSpark.Presentation.Components.Content.Rendering;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Sequences;

sealed class PreRenderingAwareAnyBuilder<T> : ISelect<string, IDepending<IQueries<T>>>
{
	readonly IDepending<IQueries<T>> _previous;
	readonly IsPreRendering          _condition;
	readonly PreRenderingAwareAny<T> _any;

	public PreRenderingAwareAnyBuilder(IsPreRendering condition, PreRenderingAwareAny<T> any)
		: this(Any.Default, condition, any) {}

	public PreRenderingAwareAnyBuilder(IDepending<IQueries<T>> previous, IsPreRendering condition,
	                                   PreRenderingAwareAny<T> any)
	{
		_previous  = previous;
		_condition = condition;
		_any       = any;
	}

	public IDepending<IQueries<T>> Get(string parameter)
	{
		var first = _condition.Get();
		var result = first
			             ? new PreRenderAwareAny<T>(_condition, _any.Get(new(_previous, parameter)), _previous)
			             : _previous;
		return result;
	}

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