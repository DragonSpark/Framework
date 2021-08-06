using System.Collections.Generic;

namespace DragonSpark.Application.Entities.Queries
{
	public interface IToList<T> : IMaterializer<T, List<T>> {}
}