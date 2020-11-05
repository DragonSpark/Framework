using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.Entities
{
	public interface IQuerying<out T> : IQueryable<T>, IAsyncEnumerable<T> {}
}