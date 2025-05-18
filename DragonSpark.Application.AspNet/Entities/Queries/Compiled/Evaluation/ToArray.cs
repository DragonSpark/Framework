using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

sealed class ToArray<T> : IEvaluate<T, Array<T>>
{
	public static ToArray<T> Default { get; } = new();

	ToArray() {}

	public async ValueTask<Array<T>> Get(Stop<IAsyncEnumerable<T>> parameter)
	{
		var (subject, token) = parameter;
		return await subject.ToArrayAsync(token).Off();
	}
}
