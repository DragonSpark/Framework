using DragonSpark.Application.Compose;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Hosting.Azure.WebJobs
{
	sealed class QueueApplicationProfile<T> : ApplicationProfile where T : QueueHost
	{
		public static QueueApplicationProfile<T> Default { get; } = new QueueApplicationProfile<T>();

		QueueApplicationProfile() : base(Services<T>.Default.Execute, _ => {}) {}
	}

	sealed class Services<T> : ICommand<IServiceCollection> where T : QueueHost
	{
		public static Services<T> Default { get; } = new Services<T>();

		Services() {}

		public void Execute(IServiceCollection parameter)
		{
			parameter.Start<IQueueApplication>()
			         .Forward<QueueApplication>()
			         .Scoped()
			         //
			         .Then.Start<T>()
			         .Scoped();
		}
	}

	public interface IQueueApplication : IAllocated<string> {}

	sealed class QueueApplication : AllocatedOperation<string>, IQueueApplication
	{
		public QueueApplication(IApplication application) : base(Start.A.Selection<string>()
		                                                              .By.Calling(Guid.Parse)
		                                                              .Select(application)
		                                                              .Then()
		                                                              .Demote()) {}
	}

	public class QueueHost : IAllocated<string>
	{
		readonly IQueueApplication _queue;

		public QueueHost(IQueueApplication queue) => _queue = queue;

		public virtual Task Get(string parameter) => _queue.Get(parameter);
	}

	public interface IApplication : IOperation<Guid> {}
}