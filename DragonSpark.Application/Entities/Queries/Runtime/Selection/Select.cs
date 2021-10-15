using System;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Runtime.Selection;

public class Select<TIn, TFrom, TOut> : IQuery<TIn, TOut>
{
	readonly IQuery<TIn, TFrom>                        _subject;
	readonly Func<IQueryable<TFrom>, IQueryable<TOut>> _selection;

	public Select(IQuery<TIn, TFrom> subject, Func<IQueryable<TFrom>, IQueryable<TOut>> selection)
	{
		_subject   = subject;
		_selection = selection;
	}

	public IQueryable<TOut> Get(TIn parameter) => _selection(_subject.Get(parameter));
}