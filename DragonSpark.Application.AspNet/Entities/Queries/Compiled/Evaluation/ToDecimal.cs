using DragonSpark.Model.Operations;
using NetFabric.Hyperlinq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

sealed class ToDecimal : IEvaluate<decimal, decimal>
{
	public static ToDecimal Default { get; } = new();

	ToDecimal() {}

	public ValueTask<decimal> Get(Stop<IAsyncEnumerable<decimal>> parameter)
	{
		var (subject, token) = parameter;
		return subject.AsAsyncValueEnumerable().SumAsync(token);
	}
}