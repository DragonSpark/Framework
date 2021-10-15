using System.Collections.Generic;

namespace DragonSpark.Application.Entities.Queries.Runtime.Materialize;

public interface IToList<T> : IMaterializer<T, List<T>> {}