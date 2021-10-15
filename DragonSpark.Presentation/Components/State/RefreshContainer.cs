using DragonSpark.Model.Sequences.Collections;
using System.Collections.Generic;

namespace DragonSpark.Presentation.Components.State;

sealed class RefreshContainer : Membership<IRefreshAware>, IRefreshContainer
{
	public RefreshContainer(ICollection<IRefreshAware> collection) : base(collection) {}
}