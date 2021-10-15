namespace DragonSpark.Application.Entities.Queries.Runtime.Materialize;

public interface IAny<in T> : IMaterializer<T, bool> {}