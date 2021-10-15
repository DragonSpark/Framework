using DragonSpark.Model.Results;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition;

public interface ISelection<TFrom, TTo> : IResult<Expression<Func<IQueryable<TFrom>, IQueryable<TTo>>>> {}