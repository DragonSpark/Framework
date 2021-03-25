namespace DragonSpark.Diagnostics
{
	public sealed class DefaultRetryPolicy<T> : RetryPolicy<T>
	{
		public static DefaultRetryPolicy<T> Default { get; } = new DefaultRetryPolicy<T>();

		DefaultRetryPolicy() {}
	}

	public sealed class DefaultRetryPolicy : RetryPolicy
	{
		public static DefaultRetryPolicy Default { get; } = new DefaultRetryPolicy();

		DefaultRetryPolicy() {}
	}
}