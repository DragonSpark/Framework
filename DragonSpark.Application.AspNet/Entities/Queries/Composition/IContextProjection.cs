using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DragonSpark.Application.AspNet.Entities.Queries.Composition;

public interface IContextProjection<TFrom, TTo> : IResult<Expression<Func<DbContext, TFrom, TTo>>>;