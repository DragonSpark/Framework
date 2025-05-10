using DragonSpark.Model.Selection;
using DragonSpark.Presentation.Components.State;

namespace DragonSpark.Presentation.Components.Content;

sealed class ActivityContents<T> : ISelect<IActivityReceiver, IActiveContent<T>>
{
	readonly IActiveContent<T> _previous;

	public ActivityContents(IActiveContent<T> previous) => _previous = previous;

	public IActiveContent<T> Get(IActivityReceiver parameter)
		=> new ActivityAwareActiveContent<T>(_previous, parameter);
}