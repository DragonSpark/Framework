using DragonSpark.Model.Selection;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Allocated.Stop;

sealed class ParameterBinding<T> : IAllocated<T>
{
	readonly ISelect<Stop<T>, Task> _previous;
	readonly CancellationToken           _parameter;

	public ParameterBinding(ISelect<Stop<T>, Task> previous, CancellationToken parameter)
	{
		_previous  = previous;
		_parameter = parameter;
	}

	public Task Get(T parameter) => _previous.Get(new(parameter, _parameter));
}