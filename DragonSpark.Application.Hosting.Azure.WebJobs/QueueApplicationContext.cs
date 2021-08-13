using DragonSpark.Application.Compose;
using DragonSpark.Composition;
using DragonSpark.Composition.Compose;
using Microsoft.Azure.WebJobs.Host;
using System;

namespace DragonSpark.Application.Hosting.Azure.WebJobs
{
	public sealed class QueueApplicationContext<T> where T : class, IApplication
	{
		readonly BuildHostContext _subject;

		public QueueApplicationContext(BuildHostContext subject) => _subject = subject;

		public ApplicationProfileContext HostedUsing<THost>() where THost : QueueHost
			=> HostedUsing<THost>(EmptyQueueConfiguration.Default.Execute);

		public ApplicationProfileContext HostedUsing<THost>(Action<QueuesOptions> configure) where THost : QueueHost
			=> _subject.Configure(new Hosting(configure))
			           .Apply(QueueApplicationProfile<THost>.Default)
			           .Then(x => x.Start<IApplication>().Forward<T>().Include(y => y.Dependencies).Scoped());
	}
}