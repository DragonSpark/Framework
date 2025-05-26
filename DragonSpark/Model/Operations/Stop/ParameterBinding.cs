using DragonSpark.Model.Selection;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Stop;

sealed class ParameterBinding<T> : IOperation<T>
{
	readonly ISelect<Stop<T>, ValueTask> _previous;
	readonly CancellationToken           _parameter;

	public ParameterBinding(ISelect<Stop<T>, ValueTask> previous, CancellationToken parameter)
	{
		_previous  = previous;
		_parameter = parameter;
	}

	public ValueTask Get(T parameter) => _previous.Get(new(parameter, _parameter));
}