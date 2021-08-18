using DragonSpark.Model.Operations;
using DragonSpark.Presentation.Components.State;

namespace DragonSpark.Presentation.Components.Content
{
	public sealed class ActivityAwareActiveContent<T> : Resulting<T>, IActiveContent<T>
	{
		readonly IActiveContent<T> _previous;

		public ActivityAwareActiveContent(IActiveContent<T> previous, object receiver)
			: this(previous, new ActivityAwareResulting<T>(previous, receiver)) {}

		public ActivityAwareActiveContent(IActiveContent<T> previous, IResulting<T> resulting)
			: base(resulting) => _previous = previous;

		public bool HasValue => _previous.HasValue;
	}
}