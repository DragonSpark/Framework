using DragonSpark.Application.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

public class EvaluateToSelectedCollection<TIn, TList, T> : Evaluate<TIn, T, TList>
	where TList : SelectedCollection<T>, new()
{
	protected EvaluateToSelectedCollection(IScopes scopes,
	                                       Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
		: this(new Reading<TIn, T>(scopes, expression)) {}

	protected EvaluateToSelectedCollection(IReading<TIn, T> reading)
		: base(reading, ToSelectedCollection<TList, T>.Default) {}
}