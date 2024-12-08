using DragonSpark.Model.Operations.Results;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime;

public interface IQueries<T> : IResulting<Query<T>>;