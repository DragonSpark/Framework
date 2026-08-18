using DragonSpark.Application.AspNet.Compose;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;

namespace DragonSpark.Application.Hosting.Server;

public sealed class CoreServerApplicationProfile : ApplicationProfile
{
	public static CoreServerApplicationProfile Default { get; } = new();

	CoreServerApplicationProfile() : this(x => x) {}

    public CoreServerApplicationProfile(Func<IApplicationBuilder, IApplicationBuilder> configure)
	    : this(_ => {}, new CoreApplicationConfiguration(configure).Then().Terminate().Get()) {}

    public CoreServerApplicationProfile(Action<MvcOptions> configure, ICommand<IApplicationBuilder> application)
        : base(new DefaultServiceConfiguration(configure), application) {}
}