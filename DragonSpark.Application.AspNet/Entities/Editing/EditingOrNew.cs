using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using JetBrains.Annotations;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public class EditingOrNew<TIn, T> : IEdit<TIn, T> where T : class
{
	readonly IEdit<TIn, T?>     _previous;
	readonly ISelecting<TIn, T> _create;

	protected EditingOrNew(IScopes scope, ISelecting<TIn, T> create, IQuery<TIn, T> query)
		: this(new Edit(scope, query), create) {}

	protected EditingOrNew(IEdit<TIn, T?> previous, ISelecting<TIn, T> create)
	{
		_previous = previous;
		_create   = create;
	}

	[MustDisposeResource]
	public async ValueTask<Edit<T>> Get(TIn parameter)
	{
		var (context, current) = await _previous.Await(parameter);
		if (current is null)
		{
			var subject = await _create.Await(parameter);
			context.Update(subject);
			return new(context, subject);
		}
		return new(context, current);
	}

	sealed class Edit : EditingOrDefault<TIn, T>
	{
		public Edit(IScopes scope, IQuery<TIn, T> query) : base(scope, query) {}
	}
}
