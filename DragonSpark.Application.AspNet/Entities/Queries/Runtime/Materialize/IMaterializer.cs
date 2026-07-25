using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Materialize;

public interface IMaterializer<T, TResult> : ISelecting<Stop<IQueryable<T>>, TResult>;