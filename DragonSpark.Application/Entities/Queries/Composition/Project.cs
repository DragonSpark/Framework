using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition
{
	public class Project<TFrom, TTo> : Instance<Expression<Func<DbContext, IQueryable<TFrom>, IQueryable<TTo>>>>
	{
		protected Project(Expression<Func<DbContext, IQueryable<TFrom>, IQueryable<TTo>>> instance) : base(instance) {}
	}
}
