using DragonSpark.Model;

namespace DragonSpark.Application.AspNet.Entities.Queries.Composition;

public interface IQuery<T> : IQuery<None, T>;
public interface IQuery<TIn, T> : IInstance<TIn, IQueryable<T>>;