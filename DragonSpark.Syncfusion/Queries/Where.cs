using DragonSpark.Compose;
using Syncfusion.Blazor;
using System.Threading.Tasks;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class Where<T> : IQuery<T>
{
	public static Where<T> Default { get; } = new();

	Where() : this("or") {}

	readonly string _condition;

	public Where(string condition) => _condition = condition;

	public ValueTask<Parameter<T>> Get(Parameter<T> parameter)
	{
		var (request, query, _) = parameter;
		var data = request.Where.Account()?.Count > 0
			           ? new(request, DataOperations.PerformFiltering(query, request.Where, _condition))
			           : parameter;
		var result = data.ToOperation();
		return result;
	}
}