using DragonSpark.Model;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Composition;

public interface IQuery<T> : IQuery<None, T> {}
public interface IQuery<TIn, T> : IInstance<TIn, IQueryable<T>> {}