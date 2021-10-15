namespace DragonSpark.Application.Entities.Queries.Runtime.Materialize;

public interface ILargeCount<in T> : IMaterializer<T, ulong> {}