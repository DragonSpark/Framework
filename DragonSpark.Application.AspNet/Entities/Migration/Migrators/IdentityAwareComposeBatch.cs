using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using NetFabric.Hyperlinq;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class IdentityAwareComposeBatch<TFrom, TTo> : IComposeBatch<TFrom, TTo> where TFrom : class where TTo : class
{
	readonly IComposeBatch<TFrom, TTo>                         _previous;
	readonly ISelect<DbContext, Expression<Func<TFrom, bool>>> _predicate;

	public IdentityAwareComposeBatch(IMap map, IEntityType type)
		: this(new ComposeBatch<TFrom, TTo>(map), new IdentityPredicate<TFrom, TTo>(type)) {}

	public IdentityAwareComposeBatch(IComposeBatch<TFrom, TTo> previous,
	                                 ISelect<DbContext, Expression<Func<TFrom, bool>>> predicate)
	{
		_previous  = previous;
		_predicate = predicate;
	}

	public Lease<TTo> Get(BatchInput<TFrom> parameter)
	{
		var predicate = _predicate.Get(parameter.Destination);
		var input     = parameter with { From = parameter.From.Where(predicate) };
		var result    = _previous.Get(input);
		return result;
	}
}