using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Materialize;

public interface IMaterializer<T, TResult> : ISelecting<Stop<IQueryable<T>>, TResult>;