using DragonSpark.Model;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition
{
	public interface IQuery<T> : IQuery<None, T> {}
	public interface IQuery<TIn, T> : IResult<Expression<Func<DbContext, TIn, IQueryable<T>>>> {}
}