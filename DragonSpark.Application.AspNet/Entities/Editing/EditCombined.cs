using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Model.Sequences.Memory;
using JetBrains.Annotations;

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
	public async ValueTask<ManyEdit<T>> Get(TIn parameter)
	{
		var (editor, first) = await _first.Await(parameter);
		var (_, second)     = await _second.Await(parameter);
		var combined = first.Then().Concat(second).Result();
		second.Dispose();
		return new(editor, combined);
	}
}
