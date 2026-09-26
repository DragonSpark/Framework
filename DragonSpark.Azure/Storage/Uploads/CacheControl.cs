using DragonSpark.Text;

namespace DragonSpark.Azure.Storage.Uploads;

sealed class CacheControl : Formatter<TimeSpan>
{
	public static CacheControl Default { get; } = new();

	CacheControl() : base(x => x == TimeSpan.Zero
		                           ? "no-cache, no-store, must-revalidate"
		                           : $"private, max-age={x.TotalSeconds:0}") {}
}