using DragonSpark.Application.AspNet.Compose;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;

namespace DragonSpark.Application.Hosting.Server;

public sealed class CoreServerApplicationProfile : ApplicationProfile
{
    public static CoreServerApplicationProfile Default { get; } = new();

    CoreServerApplicationProfile() : this(_ => {}, CoreApplicationConfiguration.Default.Then().Terminate().Get()) {}

    public CoreServerApplicationProfile(Action<MvcOptions> configure, ICommand<IApplicationBuilder> application)
        : base(new DefaultServiceConfiguration(configure), application) {}
}