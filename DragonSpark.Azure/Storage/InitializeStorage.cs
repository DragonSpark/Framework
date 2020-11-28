using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NetFabric.Hyperlinq;

namespace DragonSpark.Azure.Storage
{
	public sealed class InitializeStorage : ICommand<IApplicationBuilder>
	{
		public static InitializeStorage Default { get; } = new InitializeStorage();

		InitializeStorage() {}

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