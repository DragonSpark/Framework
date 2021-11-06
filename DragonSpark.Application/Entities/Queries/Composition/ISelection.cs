using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Composition;

public interface ISelection<TFrom, TTo> : IProjection<IQueryable<TFrom>, IQueryable<TTo>> {}