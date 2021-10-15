namespace DragonSpark.Application.Entities.Queries.Runtime.Materialize;

public interface ICount<in T> : IMaterializer<T, uint> {}