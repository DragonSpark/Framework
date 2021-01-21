using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Syncfusion.Queries
{
	sealed class Count<T> : IQuery<T>
	{
		public static Count<T> Default { get; } = new Count<T>();

		Count() {}

		public async ValueTask<Parameter<T>> Get(Parameter<T> parameter)
		{
			var (request, query, count) = parameter;
			return count is null && request.RequiresCounts ? new(request, query, (uint)await query.CountAsync()) : parameter;
		}
	}
}