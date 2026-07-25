using DragonSpark.Model.Sequences.Collections;

namespace DragonSpark.Presentation.Components.State;

sealed class RefreshContainer : Membership<IRefreshAware>, IRefreshContainer
{
	public RefreshContainer(ICollection<IRefreshAware> collection) : base(collection) {}
}