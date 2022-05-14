using DragonSpark.Application.Compose;

namespace DragonSpark.Application.Hosting.Azure.WebJobs;

sealed class QueueApplicationProfile<T> : ApplicationProfile where T : QueueHost // TODO: remove
{
	public static QueueApplicationProfile<T> Default { get; } = new();

	QueueApplicationProfile() : base(Services<T>.Default.Execute, _ => {}) {}
}