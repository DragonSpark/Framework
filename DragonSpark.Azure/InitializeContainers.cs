using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NetFabric.Hyperlinq;

namespace DragonSpark.Azure
{
	public sealed class InitializeContainers : ICommand<IApplicationBuilder>
	{
		public static InitializeContainers Default { get; } = new InitializeContainers();

		InitializeContainers() {}

		public void Execute(IApplicationBuilder parameter)
		{
			foreach (var container in parameter.ApplicationServices.GetServices<IContainer>()
			                                   .AsValueEnumerable()
			                                   .Select(x => x.Get()))
			{
				container!.CreateIfNotExists();
			}
		}
	}
}