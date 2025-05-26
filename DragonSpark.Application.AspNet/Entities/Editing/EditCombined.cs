using DragonSpark.Application.AspNet.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences.Memory;
using JetBrains.Annotations;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public class EditCombined<TIn, T> : IEditMany<TIn, T>
{
	readonly IEdit<TIn, Leasing<T>> _first, _second;

	protected EditCombined(IEnlistedScopes scopes, IQuery<TIn, T> first, IQuery<TIn, T> second)
		: this(scopes.Then().Use(first).Edit.Lease(), scopes.Then().Use(second).Edit.Lease()) {}

	protected EditCombined(IEdit<TIn, Leasing<T>> first, IEdit<TIn, Leasing<T>> second)
	{
		_first  = first;
		_second = second;
	}

	[MustDisposeResource]
	public async ValueTask<ManyEdit<T>> Get(Stop<TIn> parameter)
	{
		var (editor, first) = await _first.Off(parameter);
		var (_, second)     = await _second.Off(parameter);
		var combined = first.Then().Concat(second).Result();
		second.Dispose();
		return new(editor, combined);
	}
}
