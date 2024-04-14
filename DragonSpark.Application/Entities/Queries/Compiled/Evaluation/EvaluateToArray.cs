using DragonSpark.Application.Compose.Entities.Queries;
using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

public class EvaluateToArray<T> : EvaluateToArray<None, T>, IResulting<Array<T>>
{
	public EvaluateToArray(IScopes scopes, Expression<Func<DbContext, IQueryable<T>>> expression)
		: base(scopes, expression.Then()) {}

	public EvaluateToArray(IScopes scopes, Expression<Func<DbContext, None, IQueryable<T>>> expression)
		: base(scopes, expression) {}

	public EvaluateToArray(IReading<None, T> reading) : base(reading) {}

	public ValueTask<Array<T>> Get() => base.Get(None.Default);
}

public class EvaluateToArray<TIn, T> : Evaluate<TIn, T, Array<T>>
{
	public EvaluateToArray(IScopes scopes, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
		: this(new Reading<TIn, T>(scopes, expression)) {}

	protected EvaluateToArray(IReading<TIn, T> reading) : base(reading, ToArray<T>.Default) {}
}

// TODO
public readonly record struct Paged<TIn, T>(ISelecting<TIn, Array<T>> Page, ISelecting<TIn, uint> Count);

public interface IPageQueryComposer<TIn, T> : ISelect<PageRequest, Paged<TIn, T>>;

sealed class PageQueryComposer<TIn, T, TOther> : IPageQueryComposer<TIn, T> where TOther : class
{
	readonly IScopes               _scopes;
	readonly QueryComposer<TIn, T> _query;

	public PageQueryComposer(IScopes scopes, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
		: this(scopes, (IQuery<TIn, T>)new Query<TIn, T>(expression)) {}

	public PageQueryComposer(IScopes scopes, IQuery<TIn, T> query) : this(scopes, query.Then()) {}

	public PageQueryComposer(IScopes scopes, QueryComposer<TIn, T> query)
	{
		_scopes = scopes;
		_query  = query;
	}

	public Paged<TIn, T> Get(PageRequest parameter)
	{
		var filtered = parameter.Filter is not null ? _query.Select(x => x.Where(parameter.Filter)) : _query;
		var all      = parameter.OrderBy is not null ? filtered.Select(x => x.OrderBy(parameter.OrderBy)) : filtered;
		var paged    = all.Skip(parameter.Skip ?? 0).Take(parameter.Top ?? 10);
		var count    = new Count<TIn, T, TOther>(paged);
		return new(_scopes.Then().Use(paged).To.Array(), _scopes.Then().Use(count).To.First());
	}
}

sealed class Count<TIn, T, TOther> : Query<TIn, uint> where TOther : class
{
	public Count(Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
		: base((d, @in) => d.Set<TOther>().Select(x => (uint)expression.Invoke(d, @in).Count())) {}
}

public class EvaluateToPage<TIn, T, TOther> : ISelecting<PageRequest<TIn>, PageResponse<T>> where TOther : class
{
	readonly IPageQueryComposer<TIn, T> _query;

	protected EvaluateToPage(IScopes scopes, IQuery<TIn, T> query)
		: this(new PageQueryComposer<TIn, T, TOther>(scopes, query)) {}

	protected EvaluateToPage(IScopes scopes, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
		: this(new PageQueryComposer<TIn, T, TOther>(scopes, expression)) {}

	protected EvaluateToPage(IPageQueryComposer<TIn, T> query) => _query  = query;

	public async ValueTask<PageResponse<T>> Get(PageRequest<TIn> parameter)
	{
		var (p, c) = _query.Get(parameter);
		var page  = await p.Await(parameter.Input);
		var count = parameter.Count ? await c.Await(parameter.Input) : (uint?)null;
		return new(page.Open(), count);
	}
}

public record PageRequest<T>(
	T Input,
	bool Count = true,
	uint? Top = null,
	uint? Skip = null,
	string? Filter = null,
	string? OrderBy = null)
	: PageRequest(Count, Top, Skip, Filter, OrderBy);

public sealed record PageResponse<T>(IReadOnlyCollection<T> Page, uint? Count);

public record PageRequest(
	bool Count = true,
	uint? Top = null,
	uint? Skip = null,
	string? Filter = null,
	string? OrderBy = null);

// TODO

public sealed class DefaultPageRequest : DragonSpark.Model.Results.Instance<PageRequest>
{
	public static DefaultPageRequest Default { get; } = new();

	DefaultPageRequest() : base(new()) {}
}