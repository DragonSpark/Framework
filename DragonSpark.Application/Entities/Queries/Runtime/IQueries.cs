using DragonSpark.Model.Operations.Results;

namespace DragonSpark.Application.Entities.Queries.Runtime;

public interface IQueries<T> : IResulting<Query<T>> {}