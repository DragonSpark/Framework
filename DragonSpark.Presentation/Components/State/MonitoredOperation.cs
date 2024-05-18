using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State;

public sealed class MonitoredOperation : IOperation
{
	readonly IOperation _previous;
	readonly Switch     _subject;

	public MonitoredOperation(IOperation previous, Switch subject)
	{
		_previous = previous;
		_subject  = subject;
	}

	public async ValueTask Get()
	{
		using var _ = _subject.Scoped();
		await _previous.Await();
	}
}