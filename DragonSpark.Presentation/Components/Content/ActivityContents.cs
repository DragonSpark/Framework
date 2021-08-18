using DragonSpark.Model.Selection;

namespace DragonSpark.Presentation.Components.Content
{
	sealed class ActivityContents<T> : ISelect<object, IActiveContent<T>>
	{
		readonly IActiveContent<T> _previous;

		public ActivityContents(IActiveContent<T> previous) => _previous = previous;

		public IActiveContent<T> Get(object parameter) => new ActivityAwareActiveContent<T>(_previous, parameter);
	}
}