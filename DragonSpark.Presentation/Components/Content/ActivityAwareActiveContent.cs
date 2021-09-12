using DragonSpark.Model.Operations;
using DragonSpark.Presentation.Components.State;

namespace DragonSpark.Presentation.Components.Content
{
	public sealed class ActivityAwareActiveContent<T> : Resulting<T>, IActiveContent<T>
	{
		public ActivityAwareActiveContent(IActiveContent<T> previous, object receiver)
			: this(new ActivityAwareResulting<T>(previous, receiver)) {}

		public ActivityAwareActiveContent(IResulting<T> resulting) : base(resulting) {}
	}
}