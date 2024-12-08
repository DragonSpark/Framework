using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Queries.Composition;

public interface ISelection<TFrom, TTo> : IProjection<IQueryable<TFrom>, IQueryable<TTo>>;