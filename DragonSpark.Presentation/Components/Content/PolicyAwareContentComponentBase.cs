namespace DragonSpark.Presentation.Components.Content
{
	/*public abstract class PolicyAwareContentComponentBase<T> : ContentComponentBase<T>
	{
		protected PolicyAwareContentComponentBase() : this(Policy.Handle<Exception>().RetryAsync()) {}

		protected PolicyAwareContentComponentBase(IAsyncPolicy policy)
			=> Contents = new PolicyAwareActiveContents<T>(policy);
	}

	public abstract class
		PolicyAwareOwningContentComponentBase<TService, TContent> : OwningContentComponentBase<TService, TContent>
		where TService : class
	{
		protected PolicyAwareOwningContentComponentBase() : this(Policy.Handle<Exception>().RetryAsync()) {}

		protected PolicyAwareOwningContentComponentBase(IAsyncPolicy policy)
			=> Contents = new PolicyAwareActiveContents<TContent>(policy);

	}*/
}