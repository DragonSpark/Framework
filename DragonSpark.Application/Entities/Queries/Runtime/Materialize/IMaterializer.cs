using DragonSpark.Model.Operations.Selection;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Runtime.Materialize;

public interface IMaterializer<in T, TResult> : ISelecting<IQueryable<T>, TResult> {}