namespace DragonSpark.Azure.Storage.Uploads;

sealed class DefaultCacheControl : Text.Text
{
	public static DefaultCacheControl Default { get; } = new();

	DefaultCacheControl() : base(CacheControl.Default.Get(DefaultRequestExpiration.Default)) {}
}