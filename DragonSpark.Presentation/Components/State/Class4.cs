using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences.Collections;
using System.Collections.Generic;

namespace DragonSpark.Presentation.Components.State
{
	public interface IRefreshContainer : IMembership<IRefreshAware> {}

	sealed class RefreshContainer : Membership<IRefreshAware>, IRefreshContainer
	{
		public RefreshContainer(ICollection<IRefreshAware> collection) : base(collection) {}
	}

	public interface IRefreshAware : IAllocated {}
}