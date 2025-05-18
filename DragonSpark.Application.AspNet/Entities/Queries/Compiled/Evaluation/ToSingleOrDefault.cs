using DragonSpark.Model.Operations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

sealed class ToSingleOrDefault<T> : IEvaluate<T, T?>
{
	public static ToSingleOrDefault<T> Default { get; } = new();

	ToSingleOrDefault() {}

	public ValueTask<T?> Get(Stop<IAsyncEnumerable<T>> parameter)
		=> parameter.Subject.SingleOrDefaultAsync(parameter.Token);
}