using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.Entities.Queries.Runtime.Materialize;

public interface IToArray<T> : IMaterializer<T, Array<T>> {}