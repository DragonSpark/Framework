using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Hosting.Azure.WebJobs
{
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
}