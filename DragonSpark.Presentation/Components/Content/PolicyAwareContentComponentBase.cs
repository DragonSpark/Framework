using Polly;
using System;

namespace DragonSpark.Presentation.Components.Content
{
    public abstract class PolicyAwareContentComponentBase<T> : ContentComponentBase<T>
	{
		protected PolicyAwareContentComponentBase() : this(Policy.Handle<Exception>().RetryAsync()) {}

		protected PolicyAwareContentComponentBase(IAsyncPolicy policy)
			=> Contents = new PolicyAwareActiveContents<T>(policy);
	}
}