using DragonSpark.Application.AspNet.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using JetBrains.Annotations;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public class EditingOrNew<TIn, T> : IEdit<TIn, T> where T : class
{
	readonly IEdit<TIn, T?>     _previous;
	readonly IStopAware<TIn, T> _create;

	protected EditingOrNew(IScopes scope, IStopAware<TIn, T> create, IQuery<TIn, T> query)
		: this(new Edit(scope, query), create) {}

	protected EditingOrNew(IEdit<TIn, T?> previous, IStopAware<TIn, T> create)
	{
		_previous = previous;
		_create   = create;
	}

	[MustDisposeResource]
	public async ValueTask<Edit<T>> Get(Stop<TIn> parameter)
	{
		var (context, current) = await _previous.Off(parameter);
		if (current is null)
		{
			var subject = await _create.Off(parameter);
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
