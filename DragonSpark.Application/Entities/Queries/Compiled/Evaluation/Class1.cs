using DragonSpark.Model.Operations.Selection;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

class Class1;

public class EvaluateToPage<TIn, T> : ISelecting<PageRequest<TIn>, PageResponse<T>>
{
	readonly IScopes                                         _scopes;
	readonly Expression<Func<DbContext, TIn, IQueryable<T>>> _expression;

	protected EvaluateToPage(IScopes scopes, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
	{
		_scopes     = scopes;
		_expression = expression;
	}

	public async ValueTask<PageResponse<T>> Get(PageRequest<TIn> parameter)
	{
		using var scope    = _scopes.Get();
		var       query    = _expression.Invoke(scope.Owner, parameter.Subject).AsExpandable();
		var       filtered = parameter.Filter is not null ? query.Where(parameter.Filter) : query;
		var       all      = parameter.OrderBy is not null ? filtered.OrderBy(parameter.OrderBy) : filtered;
		var       count    = parameter.Count ? (uint)await all.CountAsync().ConfigureAwait(false) : (uint?)null;
		var       paged    = all.Skip((int)(parameter.Skip ?? 0)).Take(parameter.Top ?? 10);
		var       page     = await paged.ToArrayAsync().ConfigureAwait(false);
		return new(page, count);
	}
}

public record PageRequest<T>(
	T Subject,
	bool Count = true,
	byte? Top = null,
	uint? Skip = null,
	string? Filter = null,
	string? OrderBy = null)
	: PageRequest(Count, Top, Skip, Filter, OrderBy);

public sealed record PageResponse<T>(ICollection<T> Page, uint? Count);

[ModelBinder(BinderType = typeof(PageRequestBinder))]
public record PageRequest(
	bool Count = true,
	byte? Top = null,
	uint? Skip = null,
	string? Filter = null,
	string? OrderBy = null)
{
	public PageRequest<T> Input<T>(T parameter) => new(parameter, Count, Top, Skip, Filter, OrderBy);
}

public class PageRequestBinder : IModelBinder
{
	public Task BindModelAsync(ModelBindingContext bindingContext)
	{
		var provider = bindingContext.ValueProvider;
		var count    = provider.GetValue(nameof(PageRequest.Count));
		var top      = provider.GetValue(nameof(PageRequest.Top));
		var skip     = provider.GetValue(nameof(PageRequest.Skip));
		var request = new PageRequest(count.Length > 0,
		                              top.Length > 0 && byte.TryParse(top.FirstValue, out var t) ? t : null,
		                              skip.Length > 0 && uint.TryParse(skip.FirstValue, out var s) ? s : null,
		                              provider.GetValue(nameof(PageRequest.Filter)).FirstValue,
		                              provider.GetValue(nameof(PageRequest.OrderBy)).FirstValue
		                             );
		bindingContext.Result = ModelBindingResult.Success(request);
		return Task.CompletedTask;
	}
}

// TODO

public sealed class DefaultPageRequest : DragonSpark.Model.Results.Instance<PageRequest>
{
	public static DefaultPageRequest Default { get; } = new();

	DefaultPageRequest() : base(new()) {}
}