using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class PreRenderActiveContent<T> : Validated<ValueTask<T?>>, IActiveContent<T>
{
	readonly IActiveContent<T> _previous;

	public PreRenderActiveContent(ICondition condition, IActiveContent<T> memory, IActiveContent<T> previous)
		: base(condition, memory, previous)
		=> _previous = previous;

	public IUpdateMonitor Monitor => _previous.Monitor;

	public void Execute(T parameter)
	{
		_previous.Execute(parameter);
	}
}