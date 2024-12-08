using DragonSpark.Model.Results;
using System;
using System.Linq.Expressions;

namespace DragonSpark.Application.AspNet.Entities.Queries.Composition;

public interface IProjection<TFrom, TTo> : IResult<Expression<Func<TFrom, TTo>>>;