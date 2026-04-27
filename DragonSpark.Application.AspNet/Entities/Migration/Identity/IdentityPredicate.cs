using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Immutable;
using System.Linq.Expressions;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

sealed class IdentityPredicate<TFrom, TTo> : Select<DbContext, Expression<Func<TFrom, bool>>>
	where TFrom : class where TTo : class
{
	public IdentityPredicate(IEntityType type) : this(KnownKeys<TTo>.Default, new IdentityPredicateBody<TFrom>(type)) {}

	public IdentityPredicate(ISelect<DbContext, ImmutableHashSet<object>> keys,
	                         ISelect<ImmutableHashSet<object>, Expression<Func<TFrom, bool>>> body)
		: base(keys.Then().Select(body)) {}
}