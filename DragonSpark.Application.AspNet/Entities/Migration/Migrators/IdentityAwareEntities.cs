using DragonSpark.Application.AspNet.Entities.Migration.Identity;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class IdentityAwareEntities<TFrom, TTo> : IEntities<TFrom, TTo> where TFrom : class where TTo : class
{
	readonly IEntities<TFrom, TTo>                             _previous;
	readonly ISelect<DbContext, Expression<Func<TFrom, bool>>> _predicate;

	public IdentityAwareEntities(IMap map, IEntityType type)
		: this(new New<TFrom, TTo>(map), new IdentityPredicate<TFrom, TTo>(type)) {}

	public IdentityAwareEntities(IEntities<TFrom, TTo> previous,
	                             ISelect<DbContext, Expression<Func<TFrom, bool>>> predicate)
	{
		_previous  = previous;
		_predicate = predicate;
	}

	public IQueryable<TTo> Get(Stop<ProcessChangesInput<TFrom>> parameter)
	{
		var (subject, stop) = parameter;
		var predicate = _predicate.Get(subject.Destination);
		var input     = subject with { From = subject.From.Where(predicate) };
		var result    = _previous.Get(input.Stop(stop));
		return result;
	}
}