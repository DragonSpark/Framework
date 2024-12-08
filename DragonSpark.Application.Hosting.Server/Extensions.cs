using DragonSpark.Application.AspNet;
using DragonSpark.Application.AspNet.Compose;
using DragonSpark.Application.Compose;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DragonSpark.Application.Hosting.Server;

public static class Extensions
{
	public static ApplicationProfileContext WithServerApplication(this BuildHostContext @this)
		=> @this.Apply(ServerApplicationProfile.Default);

	public static ApplicationProfileContext WithServerApplication(this BuildHostContext @this,
	                                                              ICommand<MvcOptions> controllers,
	                                                              ICommand<IApplicationBuilder> application)
		=> @this.WithServerApplication(controllers.Execute, application);

	public static ApplicationProfileContext WithServerApplication(this BuildHostContext @this,
	                                                              ICommand<IApplicationBuilder> application)
		=> @this.WithServerApplication(_ => {}, application);

	public static ApplicationProfileContext WithServerApplication(this BuildHostContext @this,
	                                                              Action<MvcOptions> configure,
	                                                              ICommand<IApplicationBuilder> application)
		=> @this.Apply(new ServerApplicationProfile(configure, application));
}