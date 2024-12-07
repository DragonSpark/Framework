using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition;

public class StartIntroduce<TFrom, TOther, TTo> : Introduce<TFrom, TOther, TTo> where TFrom : class
{
	protected StartIntroduce(Expression<Func<DbContext, IQueryable<TOther>>> other,
	                         Expression<Func<IQueryable<TFrom>, IQueryable<TOther>, IQueryable<TTo>>> select)
		: base(Set<TFrom>.Default.Then(), other, select) {}

	protected StartIntroduce(Expression<Func<DbContext, IQueryable<TOther>>> other,
	                         Expression<Func<DbContext, IQueryable<TFrom>, IQueryable<TOther>, IQueryable<TTo>>>
		                         select)
		: base(Set<TFrom>.Default.Then(), other, select) {}
}