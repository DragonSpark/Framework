using DragonSpark.Model.Operations.Results.Stop;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime;

public interface IQueries<T> : IStopAware<Query<T>>;