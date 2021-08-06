using System.Collections.Generic;

namespace DragonSpark.Application.Entities.Queries.Materialization
{
	public interface IToList<T> : IMaterializer<T, List<T>> {}
}