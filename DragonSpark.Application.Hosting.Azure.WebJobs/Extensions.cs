using DragonSpark.Composition.Compose;

namespace DragonSpark.Application.Hosting.Azure.WebJobs
{
	public static class Extensions
	{
		public static QueueApplicationContext<T> WithQueueApplication<T>(this BuildHostContext @this)
			where T : class, IApplication => new QueueApplicationContext<T>(@this);
	}
}