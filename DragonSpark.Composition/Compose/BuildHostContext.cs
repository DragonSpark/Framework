using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Operations;
using DragonSpark.Runtime.Activation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Composition.Compose
{
	public sealed class BuildHostContext : Instance<IAlteration<IHostBuilder>>,
										   IOperationResult<HostBuilder, IHost>,
	                                       IActivateUsing<IAlteration<IHostBuilder>>
	{
		readonly IAlteration<IHostBuilder> _select;

		public BuildHostContext(IAlteration<IHostBuilder> select) : base(select) => _select = @select;

		public BuildHostContext WithEnvironment(string name) => Select(ConfigureEnvironment.Defaults.Get(name));

		public BuildHostContext Configure(ICommand<IHostBuilder> configuration)
			=> Select(configuration.Then().ToConfiguration().Out());

		public BuildHostContext Configure(ICommand<IServiceCollection> configure) => Configure(configure.Execute);

		public BuildHostContext Configure(Action<IServiceCollection> configure) => Select(new Configure(configure));

		public BuildHostContext Configure<T>() where T : ICommand<IServiceCollection>
			=> Configure(Start.A.Result<T>().By.Activation().Get().Execute);

		public BuildHostContext Select(IAlteration<IHostBuilder> select)
			=> _select.Then()
			          .Select(select)
			          .Out()
			          .To(Start.An.Extent<BuildHostContext>());

		public ValueTask<IHost> Get(HostBuilder parameter) => _select.Get(parameter)
		                                                             .StartAsync()
		                                                             .ToOperation();
	}

	public sealed class HostOperationsContext
	{
		readonly IOperationResult<HostBuilder, IHost> _select;

		public HostOperationsContext(IOperationResult<HostBuilder, IHost> select) => _select = select;

		public ValueTask<IHost> Start() => _select.Get(new HostBuilder());
	}
}