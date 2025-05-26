using DragonSpark.Model.Operations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

sealed class ToAny<T> : IEvaluate<T, bool>
{
	public static ToAny<T> Default { get; } = new();

	ToAny() {}

	public ValueTask<bool> Get(Stop<IAsyncEnumerable<T>> parameter) => parameter.Subject.AnyAsync(parameter.Token);
}