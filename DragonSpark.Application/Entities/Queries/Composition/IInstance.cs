using DragonSpark.Model;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition;

public interface IInstance<TIn, T> : IResult<Expression<Func<DbContext, TIn, T>>> {}

public interface IInstance<T> : IInstance<None, T> {}