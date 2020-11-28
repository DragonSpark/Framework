using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NetFabric.Hyperlinq;

namespace DragonSpark.Azure.Queues
{
	public sealed class InitializeQueues : ICommand<IApplicationBuilder>
	{
		public static InitializeQueues Default { get; } = new InitializeQueues();

		InitializeQueues() {}

		public void Execute(IApplicationBuilder parameter)
		{
			foreach (var client in parameter.ApplicationServices.GetServices<IQueue>()
			                                .AsValueEnumerable()
			                                .Select(x => x.Get()))
			{
				client!.CreateIfNotExists();
			}
		}
	}
}