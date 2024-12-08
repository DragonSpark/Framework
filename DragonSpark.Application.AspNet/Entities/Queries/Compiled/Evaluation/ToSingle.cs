using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

sealed class ToSingle<T> : IEvaluate<T, T>
{
	public static ToSingle<T> Default { get; } = new();

	ToSingle() {}

	public ValueTask<T> Get(IAsyncEnumerable<T> parameter) => parameter.SingleAsync();
}