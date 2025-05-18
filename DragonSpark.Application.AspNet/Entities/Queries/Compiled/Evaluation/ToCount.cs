using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

sealed class ToCount<T> : IEvaluate<T, uint>
{
	public static ToCount<T> Default { get; } = new();

	ToCount() {}

	public async ValueTask<uint> Get(Stop<IAsyncEnumerable<T>> parameter)
	{
		var (subject, token) = parameter;
		var count        = await subject.CountAsync(token).Off();
		var result       = count.Grade();
		return result;
	}
}
