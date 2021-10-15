using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Entities.Queries.Runtime;

public interface IQueries<T> : IResulting<Query<T>> {}