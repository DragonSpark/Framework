using DragonSpark.Application.AspNet;
using DragonSpark.Application.AspNet.Compose;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;

namespace DragonSpark.Application.Hosting.Server;

public static class Extensions
{
    extension(BuildHostContext @this)
    {
	    public ApplicationProfileContext WithApiApplication() => @this.Apply(CoreServerApplicationProfile.Default);

	    public ApplicationProfileContext WithApiApplication(Func<IApplicationBuilder, IApplicationBuilder> configure)
		    => @this.Apply(new CoreServerApplicationProfile(configure));

	    public ApplicationProfileContext WithServerApplication() => @this.Apply(ServerApplicationProfile.Default);

	    public ApplicationProfileContext WithServerApplication(ICommand<MvcOptions> controllers,
	                                                           ICommand<IApplicationBuilder> application)
		    => @this.WithServerApplication(controllers.Execute, application);

	    public ApplicationProfileContext WithServerApplication(ICommand<IApplicationBuilder> application)
		    => @this.WithServerApplication(_ => {}, application);

	    public ApplicationProfileContext WithServerApplication(Action<MvcOptions> configure,
	                                                           ICommand<IApplicationBuilder> application)
		    => @this.Apply(new ServerApplicationProfile(configure, application));
    }
}