using System;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Runtime.Activation;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Composition.Compose;

public sealed class BuildHostContext : ISelecting<HostBuilder, IHost>, IActivateUsing<IAlteration<IHostBuilder>>
{
    public static implicit operator Func<IHostBuilder, IHostBuilder>(BuildHostContext context) => context._select.Get;

    readonly IAlteration<IHostBuilder> _select;

    public BuildHostContext() : this(A.Self<IHostBuilder>()) { }

    public BuildHostContext(IAlteration<IHostBuilder> select) => _select = select;

    public BuildHostContext WithEnvironment(string name) => Select(new ConfigureEnvironment(name));

    public BuildHostContext Configure(Action<IHostBuilder> configuration)
        => Configure(Start.A.Command<IHostBuilder>().By.Calling(configuration).Get());

    public BuildHostContext Configure(ICommand<IHostBuilder> configuration)
        => Select(configuration.Then().ToConfiguration().Out());

    public BuildHostContext Configure(params ICommand<IServiceCollection>[] configurations)
        => Configure(new Commands<IServiceCollection>(configurations));

    public BuildHostContext Configure(ICommand<IServiceCollection> configure) => Configure(configure.Execute);

    public BuildHostContext Configure(Action<IServiceCollection> configure) => Select(new Configure(configure));

    public BuildHostContext Configure<T>() where T : ICommand<IServiceCollection>
        => Configure(Start.A.Result<T>().By.Activation().Instance().Execute);

    public BuildHostContext Select(IAlteration<IHostBuilder> select) => new(_select.Then().Select(select).Out());

    [MustDisposeResource]
    public ValueTask<IHost> Get(HostBuilder parameter) => _select.Get(parameter).StartAsync().ToOperation();
}
